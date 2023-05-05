using System;
using System.IO;
using EmbASP;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using TurnPhases.AI;
using UnityEngine;

namespace player
{
    [CreateAssetMenu(fileName = "new Bot Brain", menuName = "BotBrain", order = 0)]
    public class BotBrain : ScriptableObject
    {
        [SerializeField] private string _brainFilePath;
        
        public IAIPhase CurrentPhase => _currentPhase;
        private IAIPhase _currentPhase;

    
        public ReinforceAIPhase reinforcePhase { get; private set; }
        public AttackAIPhase attackPhase { get; private set; }
        public FortifyAIPhase fortifyPhase { get; private set; }
        public EmptyAIPhase emptyPhase { get; private set; }


        private void Awake()
        {
            SetupPhases();

            LoadExecutable();
            LoadBrainFile();
        }

        private void SetupPhases()
        {
            reinforcePhase = new ReinforceAIPhase();
            attackPhase = new AttackAIPhase();
            fortifyPhase = new FortifyAIPhase();
            emptyPhase = new EmptyAIPhase();
        }

        void LoadBrainFile()
        {
            
        }
        
        void LoadExecutable()
        {
            
        }
        
        
        
        
    }
}