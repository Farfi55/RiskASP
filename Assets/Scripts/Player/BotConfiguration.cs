using System.Collections.Generic;
using UnityEngine;

namespace player
{
    [CreateAssetMenu(fileName = "BotConfiguration", menuName = "BotConfiguration", order = 1)]
    public class BotConfiguration : ScriptableObject
    {
        public string Name => _name;
        [SerializeField] private string _name;
        
        public List<string> CommonBrainsPaths => _commonBrainsPaths;
        [SerializeField] private List<string> _commonBrainsPaths;
        
        public string ReinforceBrainPath => _reinforceBrainPath;
        [Space(10)] [SerializeField] private string _reinforceBrainPath;
        
        public string AttackBrainPath => _attackBrainPath;
        [SerializeField] private string _attackBrainPath;
        
        public string FortifyBrainPath => _fortifyBrainPath;
        [SerializeField] private string _fortifyBrainPath;
        public bool UseOptimalAnswerSet => _useOptimalAnswerSet;
        [SerializeField] private bool _useOptimalAnswerSet;
    }
}