using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using EmbASP.predicates;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using it.unical.mat.embasp.platforms.desktop;
using it.unical.mat.embasp.specializations.dlv2.desktop;
using Map;
using TurnPhases;
using TurnPhases.AI;
using UnityEngine;
using UnityEngine.Serialization;

namespace player
{
    public class BotBrain : MonoBehaviour
    {
        private GameManager _gm;

        public IAIPhase CurrentPhase { get; private set; }
        private Handler _handler;


        public Action<InputProgram> OnProgramLoaded;
        public Action<InputProgram> OnPhaseInfoLoaded;
        public Action<AnswerSet> OnResponseLoaded;

        public ReinforceAIPhase ReinforcePhase { get; private set; }
        public AttackAIPhase AttackPhase { get; private set; }
        public FortifyAIPhase FortifyPhase { get; private set; }
        public EmptyAIPhase EmptyPhase { get; private set; }


        private void Awake()
        {
            Debug.Log("BotBrain Awake");

            _gm = GameManager.Instance;
            SetupPhases();
            _handler = LoadExecutable();
            RegisterClassesToMapper();
        }
        

        private void SetupPhases()
        {
            var ar = ActionReader.Instance;
            var tr = TerritoryRepository.Instance;

            ReinforcePhase = new ReinforceAIPhase(_gm, ar, tr);
            AttackPhase = new AttackAIPhase(_gm, ar, tr);
            FortifyPhase = new FortifyAIPhase(_gm, ar, tr);
            EmptyPhase = new EmptyAIPhase();
            CurrentPhase = EmptyPhase;
        }

        // called from BotPlayer
        public void OnTurnPhaseChanged(IPhase oldPhase, IPhase newPhase)
        {
            var phase = TurnPhaseToAIPhase(newPhase);
            SetCurrentPhase(phase);
        }

        private IAIPhase TurnPhaseToAIPhase(IPhase newPhase)
        {
            return newPhase switch
            {
                TurnPhases.ReinforcePhase => ReinforcePhase,
                TurnPhases.AttackPhase => AttackPhase,
                TurnPhases.FortifyPhase => FortifyPhase,
                _ => EmptyPhase
            };
        }

        private void SetCurrentPhase(IAIPhase phase)
        {
            CurrentPhase?.OnPhaseEnd();
            
            CurrentPhase = phase;
            
            CurrentPhase.OnPhaseStart();
        }


        public void HandleCommunication(BotPlayer botPlayer) => HandleCommunication(botPlayer, botPlayer.Player);
        public void HandleCommunication(BotPlayer botPlayer, Player player)
        {
            InputProgram inputProgram = CreateProgram(botPlayer, player);

            _handler.RemoveAll();
            _handler.AddProgram(inputProgram);
            OnProgramLoaded?.Invoke(inputProgram);

            // var callback = new PhasesCallback(this, botPlayer, player);
            // _handler.StartAsync(callback);

            
            
            var output = _handler.StartSync();
            OnResponse(botPlayer, player, output);
        }

        private class PhasesCallback : ICallback
        {
            private readonly BotBrain _botBrain;
            private readonly BotPlayer _botPlayer;
            private readonly Player _player;

            public PhasesCallback(BotBrain botBrain, BotPlayer botPlayer,  Player player)
            {
                _botBrain = botBrain;
                _botPlayer = botPlayer;
                _player = player;
            }

            public void Callback(Output output)
            {
                _botBrain.OnResponse(_botPlayer, _player, output);
            }
        }

        private void OnResponse(BotPlayer botPlayer, Player player, Output output)
        {
            if (output is not AnswerSets outputAnswerSets)
            {
                Debug.LogError("Output is not AnswerSets");
                return;
            }
            
            IList<AnswerSet> answerSets;
            if (botPlayer.BotConfiguration.UseOptimalAnswerSet)
                answerSets = outputAnswerSets.GetOptimalAnswerSets();
            else
                answerSets = outputAnswerSets.Answersets;

            
            if (answerSets.Count == 0)
            {
                Debug.LogError($"BotBrain: No answer set found {player.Name}, phase: {CurrentPhase.GetType().Name}\noutput-error: {output.ErrorsString}\noutput: {output.OutputString}" );
                CurrentPhase.OnFailure(player);
                return;
            }

            var answerSet = answerSets[0];
            OnResponseLoaded?.Invoke(answerSet);
            
            CurrentPhase.OnResponse(player, answerSet);
        }

        public InputProgram CreateProgram(BotPlayer botPlayer, Player player)
        {
            InputProgram inputProgram = new ASPInputProgram();
            
            string currentPhaseBrain = CurrentPhase switch
            {
                ReinforceAIPhase => botPlayer.BotConfiguration.ReinforceBrainPath,
                AttackAIPhase => botPlayer.BotConfiguration.AttackBrainPath,
                FortifyAIPhase => botPlayer.BotConfiguration.FortifyBrainPath,
                _ => "",
            };

            LoadPhaseInfo(inputProgram, player);
            OnPhaseInfoLoaded?.Invoke(inputProgram);
            
            LoadCommonBrains(botPlayer, inputProgram);

            LoadBrain(currentPhaseBrain, inputProgram);
            

            return inputProgram;
        }

        private void LoadPhaseInfo(InputProgram inputProgram, Player currentPlayer)
        {
            var tr = TerritoryRepository.Instance;

            // turn info
            var turn = new TurnPredicate(_gm.Turn, _gm.CurrentPlayer.Name);
            inputProgram.AddObjectInput(turn);

            // territory control
            var territoryControls = TerritoryControlPredicate.FromTerritoriesAsObjects(_gm.Turn, tr.Territories);
            inputProgram.AddObjectsInput(territoryControls);

            // territory island
            foreach (var (territory, islandId) in tr.TerritoryToIslandMap)
            {
                var territoryPredicate = new TerritoryIslandPredicate(_gm.Turn, islandId, territory.Name, territory.Owner.Name);
                inputProgram.AddObjectInput(territoryPredicate);
            }

            // players
            foreach (var player in _gm.Players)
                inputProgram.AddObjectInput(new PlayerPredicate(player.Name));
            
            CurrentPhase.OnRequest(currentPlayer, inputProgram);
        }


        private void LoadCommonBrains(BotPlayer botPlayer, InputProgram inputProgram)
        {
            foreach (var commonBrainPath in botPlayer.BotConfiguration.CommonBrainsPaths) 
                LoadBrain(commonBrainPath, inputProgram);
        }

        private void LoadBrain(string brainPath, InputProgram inputProgram)
        {
            if (brainPath == "")
                throw new Exception("Brain path not set");
            if (!File.Exists(brainPath))
                throw new IOException($"Brain file not found at {brainPath}");
            string text = File.ReadAllText(brainPath);
            inputProgram.AddProgram(text);
        }


        private Handler LoadExecutable()
        {
            var separator = Path.DirectorySeparatorChar;
            string executablePath;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                executablePath = $"Executables{separator}dlv2.exe";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                executablePath = $"Executables{separator}dlv2-linux";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                executablePath = $"Executables{separator}dlv2-mac";
            else throw new Exception("OS not supported");


            return new DesktopHandler(new DLV2DesktopService(executablePath));
        }


        void RegisterClassesToMapper()
        {
            // get all classes in the namespace EmbASP.predicates
            var predicateClasses = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace == "EmbASP.predicates")
                .ToList();

            // register all predicate classes
            foreach (var predicateClass in predicateClasses)
            {
                ASPMapper.Instance.RegisterClass(predicateClass);
            }
        }
    }
}