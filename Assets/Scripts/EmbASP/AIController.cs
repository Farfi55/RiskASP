using System.Collections.Generic;
using System.Runtime.InteropServices;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using it.unical.mat.embasp.platforms.desktop;
using it.unical.mat.embasp.specializations.dlv2.desktop;
using UnityEngine;
using Player = EmbASP.predicates.Player;

using File = UnityEngine.Windows.File;

namespace EmbASP
{
    public class AIController
    {
        private Handler _handler;
        private string ai1;
        public void ConfigAsp()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
            {
                ai1 = @".\AIs\ai3";
                _handler = new DesktopHandler(new DLV2DesktopService("./Executables/dlv2.exe"));
            }
            else{
                ai1 = @"./AIs/ai3";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    _handler = new DesktopHandler(new DLV2DesktopService("./Executables/dlv2-linux"));
                }
                else
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        _handler = new DesktopHandler(new DLV2DesktopService("./Executables/dlv2-mac"));
                    }
                }
            }
            ASPMapper.Instance.RegisterClass(typeof(Player));
            /*
            ASPMapper.Instance.RegisterClass(typeof(AfterAttackMove));
            ASPMapper.Instance.RegisterClass(typeof(Attack));
            ASPMapper.Instance.RegisterClass(typeof(AttackResult));
            ASPMapper.Instance.RegisterClass(typeof(Move));
            ASPMapper.Instance.RegisterClass(typeof(Place));
            
            ASPMapper.Instance.RegisterClass(typeof(StopAttacking));
            ASPMapper.Instance.RegisterClass(typeof(TerritoryControl));
            ASPMapper.Instance.RegisterClass(typeof(predicates.Turn));
            ASPMapper.Instance.RegisterClass(typeof(UnitsToPlace));*/

            InputProgram input = new ASPInputProgram();

            if (File.Exists(ai1))
            {
                string str = System.IO.File.ReadAllText(ai1);
                input.AddProgram(str);
                //input.AddFilesPath(ai1);  // Non funziona 
                
                
                //Debug.Log(input.Programs.ToString());
            }
            _handler.AddProgram(input);
            AnswerSets answerSets = (AnswerSets)_handler.StartSync();

            Debug.Log(answerSets.Answersets);
            List<Player> players = new List<Player>();
            foreach (AnswerSet answerSet in answerSets.Answersets)
            {
                
                foreach (object obj in answerSet.Atoms)
                {
                    if (typeof(Player).IsInstanceOfType(obj))
                    {
                        players.Add((Player)obj);
                    }
                    
                }
            }

            foreach (Player p1 in players)
            {
                Debug.Log(p1.get_name());
            }

        }
        

    }
}