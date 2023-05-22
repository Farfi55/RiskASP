using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Cards;
using EmbASP.predicates;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using it.unical.mat.embasp.platforms.desktop;
using it.unical.mat.embasp.specializations.clingo.desktop;
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

        private Handler _dlv2Handler;
        private Handler _clingoHandler;


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
            LoadExecutables();
            RegisterClassesToMapper();
        }


        private void SetupPhases()
        {
            var ar = ActionReader.Instance;
            var tr = TerritoryRepository.Instance;
            var cr = CardRepository.Instance;

            ReinforcePhase = new ReinforceAIPhase(_gm, ar, tr, cr);
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

            var currHandler = botPlayer.BotConfiguration.ASPSolver switch
            {
                ASPSolver.DLV2 => _dlv2Handler,
                ASPSolver.Clingo => _clingoHandler,
                _ => throw new ArgumentOutOfRangeException()
            };

            currHandler.RemoveAll();
            currHandler.AddProgram(inputProgram);
            OnProgramLoaded?.Invoke(inputProgram);

            // var callback = new PhasesCallback(this, botPlayer, player);
            // _handler.StartAsync(callback);


            var output = currHandler.StartSync();
            OnResponse(botPlayer, player, output);
        }

        private class PhasesCallback : ICallback
        {
            private readonly BotBrain _botBrain;
            private readonly BotPlayer _botPlayer;
            private readonly Player _player;

            public PhasesCallback(BotBrain botBrain, BotPlayer botPlayer, Player player)
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
            if (output is not AnswerSets outputAnswerSets
                || outputAnswerSets.Answersets is null
                || outputAnswerSets.Answersets.Count == 0)
            {
                Debug.LogError(GetErrorMessage(botPlayer, output));
                CurrentPhase.OnFailure(player);
                return;
            }
            if(output.ErrorsString.Length > 0)
                Debug.LogWarning(GetCurrentPhaseInfo(botPlayer) + "\n" + output.ErrorsString);


            
            IList<AnswerSet> answerSets;
            if (botPlayer.BotConfiguration.UseOptimalAnswerSet)
            {
                try
                {
                    answerSets = outputAnswerSets.GetOptimalAnswerSets();
                }
                catch (InvalidOperationException e)
                {
                    Debug.LogError(GetErrorMessage(botPlayer, output));
                    Debug.LogError(e);
                    CurrentPhase.OnFailure(player);
                    throw;
                }
            }
            else
                answerSets = outputAnswerSets.Answersets;


            if (answerSets.Count == 0)
            {
                Debug.LogError(GetErrorMessage(botPlayer, output));
                CurrentPhase.OnFailure(player);
                return;
            }
            
            var answerSet = answerSets[0];
            OnResponseLoaded?.Invoke(answerSet);

            CurrentPhase.OnResponse(player, answerSet);
        }

        private string GetErrorMessage(BotPlayer player, Output output)
        {
            var error = new StringBuilder();
            error.Append("BotBrain: No answer set found");
            error.Append(GetCurrentPhaseInfo(player));

            error.AppendLine()
                .Append("output-error: ").AppendLine(output.ErrorsString)
                .Append("output: ").AppendLine(output.OutputString);
            return error.ToString();
        }

        private string GetCurrentPhaseInfo(BotPlayer botPlayer)
        {
            var info = new StringBuilder("Player: ").Append(botPlayer.Player.Name)
                .Append(" Config: ").Append(botPlayer.BotConfiguration.Name)
                .Append(" Solver: ").Append(botPlayer.BotConfiguration.ASPSolver.ToString())
                .Append(" Phase: ").Append(CurrentPhase.GetType().Name)
                .Append(" Turn: ").Append(_gm.Turn.ToString());
            if (_gm.CurrentPhase is AttackPhase attackPhase)
            {
                info.Append(" AttackTurn: ").Append(attackPhase.AttackTurn.ToString());
                info.Append(" AttackPhaseState: ").Append(attackPhase.State.ToString());
            }

            return info.ToString();
        }

        public InputProgram CreateProgram(BotPlayer botPlayer, Player player)
        {
            InputProgram inputProgram = new ASPInputProgram();

            var currentPhaseBrains = botPlayer.BotConfiguration.BrainsPathsForPhase(CurrentPhase);

            LoadPhaseInfo(inputProgram, player);
            OnPhaseInfoLoaded?.Invoke(inputProgram);

            LoadCommonBrains(botPlayer, inputProgram);

            foreach (var currentPhaseBrain in currentPhaseBrains)
                LoadBrain(currentPhaseBrain, inputProgram);


            return inputProgram;
        }

        private void LoadPhaseInfo(InputProgram inputProgram, Player currentPlayer)
        {
            var tr = TerritoryRepository.Instance;

            // turn info
            var turn = _gm.Turn;
            var turnPredicate = new TurnPredicate(turn, currentPlayer.Name);
            inputProgram.AddObjectInput(turnPredicate);

            // territory control
            var territoryControls = TerritoryControlPredicate.FromTerritoriesAsObjects(turn, tr.Territories);
            inputProgram.AddObjectsInput(territoryControls);

            // territory island
            foreach (var (territory, islandId) in tr.TerritoryToIslandMap)
            {
                var territoryPredicate =
                    new TerritoryIslandPredicate(turn, islandId, territory.Name, territory.Owner.Name);
                inputProgram.AddObjectInput(territoryPredicate);
            }

            foreach (var player in _gm.Players)
            {
                // players
                inputProgram.AddObjectInput(new PlayerPredicate(player.Name));
                // player cards count
                inputProgram.AddObjectInput(new CardsCountPredicate(turn, player.Name, player.Cards.Count));
            }

            // current player cards
            foreach (var currentPlayerCard in currentPlayer.Cards)
                inputProgram.AddObjectInput(new CardPredicate(turn, currentPlayer.Name, currentPlayerCard));


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


        private void LoadExecutables()
        {
            var sep = Path.DirectorySeparatorChar;
            string dlv2ExecutablePath = $"Executables{sep}dlv2{sep}dlv2";
            string clingoExecutablePath = $"Executables{sep}clingo{sep}clingo";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                dlv2ExecutablePath += "-win.exe";
                clingoExecutablePath += "-win.exe";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                dlv2ExecutablePath += "-linux";
                clingoExecutablePath += "-linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                dlv2ExecutablePath += "-mac";
                clingoExecutablePath += "-mac";
            }
            else throw new Exception("OS not supported");


            _dlv2Handler = new DesktopHandler(new DLV2DesktopService(dlv2ExecutablePath));
            _clingoHandler = new DesktopHandler(new ClingoDesktopService(clingoExecutablePath));
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