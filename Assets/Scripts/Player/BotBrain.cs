using System;
using System.IO;
using System.Runtime.InteropServices;
using EmbASP;
using EmbASP.predicates;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using it.unical.mat.embasp.platforms.desktop;
using it.unical.mat.embasp.specializations.dlv2.desktop;
using Map;
using TurnPhases;
using TurnPhases.AI;
using UnityEngine;

namespace player
{
    [CreateAssetMenu(fileName = "new Bot Brain", menuName = "BotBrain", order = 0)]
    public class BotBrain : ScriptableObject
    {
        private GameManager _gm;


        [SerializeField] private string _constantsBrainPath;
        [Space(10)] [SerializeField] private string _reinforceBrainPath;
        [SerializeField] private string _attackBrainPath;
        [SerializeField] private string _fortifyBrainPath;

        public IAIPhase currentPhase { get; private set; }
        private Handler _handler;


        public ReinforceAIPhase reinforcePhase { get; private set; }
        public AttackAIPhase attackPhase { get; private set; }
        public FortifyAIPhase fortifyPhase { get; private set; }
        public EmptyAIPhase emptyPhase { get; private set; }


        private void Awake()
        {
            _gm = GameManager.Instance;
            SetupPhases();
            _handler = LoadExecutable();
            RegisterClassesToMapper();
        }


        private void SetupPhases()
        {
            var ar = ActionReader.Instance;
            var tr = TerritoryRepository.Instance;

            reinforcePhase = new ReinforceAIPhase(_gm, ar);
            attackPhase = new AttackAIPhase(_gm, ar, tr);
            fortifyPhase = new FortifyAIPhase(_gm, ar);
            emptyPhase = new EmptyAIPhase();
            currentPhase = emptyPhase;
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
                ReinforcePhase => reinforcePhase,
                AttackPhase => attackPhase,
                FortifyPhase => fortifyPhase,
                _ => emptyPhase
            };
        }

        private void SetCurrentPhase(IAIPhase phase)
        {
            currentPhase = phase;
        }


        public void HandleCommunication(Player player)
        {
            InputProgram inputProgram = CreateProgram();
            currentPhase.Start(player, inputProgram);
            _handler.AddProgram(inputProgram);
            _handler.StartAsync(new PhasesCallback(this, inputProgram, _handler));
        }

        private class PhasesCallback : ICallback
        {
            private BotBrain _botBrain;
            private InputProgram _inputProgram;
            private Handler _handler;

            public PhasesCallback(BotBrain botBrain, InputProgram inputProgram, Handler handler)
            {
                _botBrain = botBrain;
                _inputProgram = inputProgram;
                _handler = handler;
            }

            public void Callback(Output output)
            {
                _handler.RemoveProgram(_inputProgram);

                AnswerSet answerSet;
                var answerSets = (AnswerSets)output;
                var optimalAnswerSet = answerSets.GetOptimalAnswerSets();

                if (optimalAnswerSet.Count > 0)
                    answerSet = optimalAnswerSet[0];
                else answerSet = answerSets.Answersets[0];

                _botBrain.currentPhase.OnResponse(answerSet);
            }
        }

        public InputProgram CreateProgram()
        {
            string currentBrain = currentPhase switch
            {
                ReinforceAIPhase => _reinforceBrainPath,
                AttackAIPhase => _attackBrainPath,
                FortifyAIPhase => _fortifyBrainPath,
                _ => ""
            };

            InputProgram inputProgram = new ASPInputProgram();
            LoadConstants(inputProgram);

            // todo: uncomment when brains are ready
            // LoadBrain(currentBrain, inputProgram);

            return inputProgram;
        }

        private void LoadConstants(InputProgram inputProgram)
        {
            if(_constantsBrainPath == "")
                return;
            
            if (!File.Exists(_constantsBrainPath))
                throw new IOException("Constants file not found");
            string str = File.ReadAllText(_constantsBrainPath);
            inputProgram.AddProgram(str);
        }

        private void LoadBrain(string brainPath, InputProgram inputProgram)
        {
            if(brainPath == "")
                throw new Exception("Brain path not set");
            if (!File.Exists(brainPath))
                throw new IOException("Brain file not found");
            string str = File.ReadAllText(brainPath);
            inputProgram.AddProgram(str);
        }


        private Handler LoadExecutable()
        {
            var separator = Path.DirectorySeparatorChar;
            string executablePath;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                executablePath = $".{separator}Executables{separator}dlv2.exe";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                executablePath = $".{separator}Executables{separator}dlv2-linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                executablePath = $".{separator}Executables{separator}dlv2-mac";
            }
            else throw new Exception("OS not supported");

            return new DesktopHandler(new DLV2DesktopService(executablePath));
        }


        void RegisterClassesToMapper()
        {
            ASPMapper.Instance.RegisterClass(typeof(EmbASP.predicates.Player));
            ASPMapper.Instance.RegisterClass(typeof(AfterAttackMove));
            ASPMapper.Instance.RegisterClass(typeof(Attack));
            ASPMapper.Instance.RegisterClass(typeof(AttackResult));
            ASPMapper.Instance.RegisterClass(typeof(Move));
            ASPMapper.Instance.RegisterClass(typeof(Place));

            ASPMapper.Instance.RegisterClass(typeof(StopAttacking));
            ASPMapper.Instance.RegisterClass(typeof(TerritoryControl));
            ASPMapper.Instance.RegisterClass(typeof(Turn));
            ASPMapper.Instance.RegisterClass(typeof(UnitsToPlace));
        }
    }
}