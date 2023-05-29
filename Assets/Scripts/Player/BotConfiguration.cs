using System;
using System.Collections.Generic;
using TurnPhases.AI;
using UnityEngine;

namespace player
{
    [CreateAssetMenu(fileName = "BotConfiguration", menuName = "BotConfiguration", order = 1)]
    public class BotConfiguration : PlayerConfiguration
    {
        public override string Name => _name;
        
        [SerializeField] private string _name;
        
        public ASPSolver ASPSolver => _aspSolver;
        [SerializeField] private ASPSolver _aspSolver = ASPSolver.DLV2;
        
        public List<string> CommonBrainsPaths => _commonBrainsPaths;
        [SerializeField] private List<string> _commonBrainsPaths;
        
        public List<string> DraftBrainsPaths => _draftBrainsPaths;
        [Space(10)] [SerializeField] private List<string> _draftBrainsPaths;
        
        public List<string> AttackBrainsPaths => _attackBrainsPaths;
        [SerializeField] private List<string> _attackBrainsPaths;
        
        public List<string> FortifyBrainsPaths => _fortifyBrainsPaths;
        [SerializeField] private List<string> _fortifyBrainsPaths;
        public bool UseOptimalAnswerSet => _useOptimalAnswerSet;
        [SerializeField] private bool _useOptimalAnswerSet;


        public List<string> BrainsPathsForPhase(IAIPhase phase)
        {
            return phase switch
            {
                ReinforceAIPhase => DraftBrainsPaths,
                AttackAIPhase => AttackBrainsPaths,
                FortifyAIPhase => FortifyBrainsPaths,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}