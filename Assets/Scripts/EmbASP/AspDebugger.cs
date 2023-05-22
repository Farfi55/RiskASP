using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using player;
using TurnPhases;
using UnityEngine;
using UnityEngine.Serialization;

namespace EmbASP
{
    public class AspDebugger : MonoBehaviour
    {
        private GameManager _gm;
        [SerializeField] private BotBrain _botBrain;

        [FormerlySerializedAs("_dumpsPath")] [SerializeField] private string _dumpFilesPath;
        
        [Space(10)]
        [SerializeField] private bool _logPhaseToConsole;
        [SerializeField] private bool _logFullToConsole;
        [SerializeField] private bool _logResponseToConsole;
        
        [Space(10)]
        [SerializeField] private bool _logPhaseToFile;
        [SerializeField] private bool _logFullToFile;
        [SerializeField] private bool _logResponseToFile;
        
        
        [FormerlySerializedAs("_clearAllDumpsOnStart")]
        [Space(10)]
        [SerializeField] private bool _deleteOldDumpsOnStart;
        [SerializeField, Range(0, 100)] private int _maxTurnsToLog = 10;
        [SerializeField] private bool _logAllTurns = false;

        
        [Space(10), TextArea(4, 10), SerializeField] 
        private string _programPhaseText;
        
        [Space(10), TextArea(4, 10), SerializeField]
        private string _programFullText;
        
        [Space(10), TextArea(4, 10), SerializeField]
        private string _programResponseText;
        
        
        
        private void Awake()
        {
            _gm = GameManager.Instance;
            if (_deleteOldDumpsOnStart)
                ClearAllDumpsOnStart();

            if (_botBrain == null)
            {
                Debug.LogWarning("BotBrain not set, trying to find it in scene");   
                _botBrain = FindObjectOfType<BotBrain>();
            }
            
            _botBrain.OnPhaseInfoLoaded += PhaseLoaded;
            
            _botBrain.OnProgramLoaded += FullProgramLoaded;
            
            
            _botBrain.OnResponseLoaded += ResponseLoaded;
        }
        
        private void ClearAllDumpsOnStart()
        {
            // delete all files starting with 'dump' inside dump-folder
            var dumpFiles = System.IO.Directory.GetFiles(_dumpFilesPath, "dump*");
            foreach (var dumpFile in dumpFiles)
            {
                System.IO.File.Delete(dumpFile);
            }
        }


        private void FullProgramLoaded(InputProgram program)
        {
            _programFullText = program.Programs;
            var phaseName = _botBrain.CurrentPhase.GetType().Name;
            var turn = _gm.Turn;
            if (_logFullToFile && (_logAllTurns || turn <= _maxTurnsToLog))
            {
                
                
                string dumpPath;
                if(_gm.CurrentPhase is AttackPhase attackPhase)
                    dumpPath = $"{_dumpFilesPath}/dump_full_t{turn}_{phaseName}_at{attackPhase.AttackTurn}.dlv";
                else 
                    dumpPath = $"{_dumpFilesPath}/dump_full_t{turn}_{phaseName}.dlv";
                
                System.IO.File.WriteAllText(dumpPath, _programFullText);
            }

            if (_logFullToConsole && (_logAllTurns || turn <= _maxTurnsToLog))
            {
                Debug.Log($"turn {turn}, full info for {phaseName}:\n______________________\n{_programFullText}\n______________________");
            }
        }
        
        private void PhaseLoaded(InputProgram program)
        {
            _programPhaseText = program.Programs;
            
            var phaseName = _botBrain.CurrentPhase.GetType().Name;
            var turn = _gm.Turn;
            
            if (_logPhaseToFile && (_logAllTurns || turn <= _maxTurnsToLog))
            {
                string dumpPath;
                if(_gm.CurrentPhase is AttackPhase attackPhase)
                    dumpPath = $"{_dumpFilesPath}/dump_phase_t{turn}_{phaseName}_at{attackPhase.AttackTurn}.dlv";
                else 
                    dumpPath = $"{_dumpFilesPath}/dump_phase_t{turn}_{phaseName}.dlv";
                
                System.IO.File.WriteAllText(dumpPath, _programPhaseText);
            }

            if (_logPhaseToConsole && (_logAllTurns || turn <= _maxTurnsToLog))
            {
                Debug.Log($"turn {turn}, phase info for {phaseName}:\n______________________\n{_programPhaseText}\n______________________");
            }
            
        }
        
        
        private void ResponseLoaded(AnswerSet answerSet)
        {
            var sb = new StringBuilder();
            
            foreach (var (level, cost) in answerSet.Weights) 
                sb.AppendLine($"% COST {cost} @ {level}");

            foreach (var atomString in answerSet.Value)
                sb.AppendLine(atomString);

            _programResponseText = sb.ToString();
            
            var phaseName = _botBrain.CurrentPhase.GetType().Name;
            var turn = _gm.Turn;
            
            if (_logResponseToFile && (_logAllTurns || turn <= _maxTurnsToLog))
            {
                string dumpPath;
                if(_gm.CurrentPhase is AttackPhase attackPhase)
                    dumpPath = $"{_dumpFilesPath}/dump_response_t{turn}_{phaseName}_at{attackPhase.AttackTurn}.dlv";
                else 
                    dumpPath = $"{_dumpFilesPath}/dump_response_t{turn}_{phaseName}.dlv";
                
                System.IO.File.WriteAllText(dumpPath, _programResponseText);
            }
            
            if (_logResponseToConsole && (_logAllTurns || turn <= _maxTurnsToLog))
            {
                Debug.Log($"turn {turn}, response for {phaseName}:\n______________________\n{_programResponseText}\n______________________");
            }
        }
        
        
    }
}
