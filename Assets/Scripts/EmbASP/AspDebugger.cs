using System;
using it.unical.mat.embasp.@base;
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

        [SerializeField] private string _dumpsPath;
        
        [Space(10)]
        [SerializeField] private bool _logPhaseInfo;
        [SerializeField] private bool _logProgramLoadedInfo;
        
        [Space(10)]
        [SerializeField] private bool _dumpPhaseInfo;
        [SerializeField] private bool _dumpProgramLoaded;
        
        
        [Space(10)]
        [SerializeField] private bool _clearAllDumpsOnStart;
        
        
        private void Start()
        {
            _gm = GameManager.Instance;
            if (_clearAllDumpsOnStart)
                ClearAllDumpsOnStart();
            
            _botBrain.OnProgramLoaded += ProgramLoaded;
            
            _botBrain.OnPhaseInfoLoaded += PhaseInfo;
        }

        private void ClearAllDumpsOnStart()
        {
            // delete all files starting with 'dump' inside dump-folder
            var dumpFiles = System.IO.Directory.GetFiles(_dumpsPath, "dump*");
            foreach (var dumpFile in dumpFiles)
            {
                System.IO.File.Delete(dumpFile);
            }
        }


        private void ProgramLoaded(InputProgram program)
        {
            var programStr = program.Programs;
            var phaseName = _botBrain.CurrentPhase.GetType().Name;
            var turn = _gm.Turn;
            if (_dumpProgramLoaded)
            {
                
                
                string dumpPath;
                if(_gm.CurrentPhase is AttackPhase attackPhase)
                    dumpPath = $"{_dumpsPath}/dump_full_t{turn}_{phaseName}_at{attackPhase.AttackTurn}.dlv";
                else 
                    dumpPath = $"{_dumpsPath}/dump_full_t{turn}_{phaseName}.dlv";
                
                System.IO.File.WriteAllText(dumpPath, programStr);
            }

            if (_logProgramLoadedInfo)
            {
                Debug.Log($"turn {turn}, full info for {phaseName}:\n______________________\n{programStr}\n______________________");
            }
        }
        
        private void PhaseInfo(InputProgram program)
        {
            var programStr = program.Programs;
            var phaseName = _botBrain.CurrentPhase.GetType().Name;
            var turn = _gm.Turn;
            
            if (_dumpProgramLoaded)
            {
                string dumpPath;
                if(_gm.CurrentPhase is AttackPhase attackPhase)
                    dumpPath = $"{_dumpsPath}/dump_phase_t{turn}_{phaseName}_at{attackPhase.AttackTurn}.dlv";
                else 
                    dumpPath = $"{_dumpsPath}/dump_phase_t{turn}_{phaseName}.dlv";
                
                System.IO.File.WriteAllText(dumpPath, programStr);
            }

            if (_logProgramLoadedInfo)
            {
                Debug.Log($"turn {turn}, phase info for {phaseName}:\n______________________\n{programStr}\n______________________");
            }
            
        }
    }
}