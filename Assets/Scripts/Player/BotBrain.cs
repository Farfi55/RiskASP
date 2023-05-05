using System;
using System.IO;
using System.Runtime.InteropServices;
using EmbASP;
using EmbASP.predicates;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using it.unical.mat.embasp.platforms.desktop;
using it.unical.mat.embasp.specializations.dlv2.desktop;
using TurnPhases.AI;
using UnityEngine;

namespace player
{
    [CreateAssetMenu(fileName = "new Bot Brain", menuName = "BotBrain", order = 0)]
    public class BotBrain : ScriptableObject
    {
        [SerializeField] private string _brainFilePath;

        [SerializeField] private string _filePathAiRenforcement;
        [SerializeField] private string _filePathAiAttack;
        [SerializeField] private string _filePathAiFortify;
        [SerializeField] private string _filePathAiFacts;

        public IAIPhase CurrentPhase => _currentPhase;
        private IAIPhase _currentPhase;
        private Handler _handler;

    
        public ReinforceAIPhase reinforcePhase { get; private set; }
        public AttackAIPhase attackPhase { get; private set; }
        public FortifyAIPhase fortifyPhase { get; private set; }
        public EmptyAIPhase emptyPhase { get; private set; }


        private void Awake()
        {
            SetupPhases();
            LoadExecutable();
            LoadBrainFile();
            ConfigEmbAsp();
        }

        private void SetupPhases()
        {
            reinforcePhase = new ReinforceAIPhase();
            attackPhase = new AttackAIPhase();
            fortifyPhase = new FortifyAIPhase();
            emptyPhase = new EmptyAIPhase();
        }

        
        public void HandleComunication(Player p)
        {
            InputProgram input = InitAI();
            _currentPhase.Start(p, input);
            _handler.AddProgram(input);
            _handler.StartAsync(new PhasesCallback());
        }

        internal class PhasesCallback : ICallback
        {
            BotBrain _botBrain;
            public void Callback(Output o)
            {
                var answersets = (AnswerSets)o; 
                _botBrain.CurrentPhase.OnResponse(answersets.Answersets[0]);
            }
        }
        
        public InputProgram InitAI()
        {
            string currentAI = _currentPhase switch
            {
                ReinforceAIPhase _ => _filePathAiRenforcement,
                AttackAIPhase _ => _filePathAiAttack,
                FortifyAIPhase _ => _filePathAiFortify,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            InputProgram input = new ASPInputProgram();
            LoadAi(currentAI, input);

            return input;
        }
        
        private void LoadAi(string aiPath, InputProgram inputProgram)
        {
            if (File.Exists(aiPath) && File.Exists(_filePathAiFacts))
            {
                string str = System.IO.File.ReadAllText(aiPath);
                inputProgram.AddProgram(str);
                str = System.IO.File.ReadAllText(_filePathAiFacts);
                inputProgram.AddProgram(str);
            }
        }
        
        
        void LoadBrainFile()
        {
            var separator = Path.DirectorySeparatorChar;
            _filePathAiRenforcement = $@".{separator}AIs{separator}ReinforcementAI";
            _filePathAiAttack = $@".{separator}AIs{separator}AttackAI";
            _filePathAiFortify = $@".{separator}AIs{separator}FortifyAI";
            _filePathAiFacts = $@".{separator}AIs{separator}Facts";
        }
        
        
        
        void LoadExecutable()
        {
            var separator = Path.DirectorySeparatorChar;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
            {

                _handler = new DesktopHandler(new DLV2DesktopService($".{separator}Executables{separator}dlv2.exe"));
            }
            else{

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    _handler = new DesktopHandler(new DLV2DesktopService($".{separator}Executables{separator}dlv2-linux"));
                }
                else
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        _handler = new DesktopHandler(new DLV2DesktopService($".{separator}Executables{separator}dlv2-mac"));
                    }
                }
            }
        }





        void ConfigEmbAsp()
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