using System;
using EmbASP.predicates;
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
        private Handler _handler = new DesktopHandler(new DLV2DesktopService("../../Executables/dlv2.exe"));
        public void ConfigAsp()
        {
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
            
            string ai1 = @"./AIs/ai1.dlv";

            //if (File.Exists(ai1))
            //{
                //string str = System.IO.File.ReadAllText(ai1);
                //input.AddProgram(str);
                input.AddFilesPath(ai1);
            //}
            _handler.AddProgram(input);
            AnswerSets answerSets = (AnswerSets)_handler.StartSync();

            Debug.Log(answerSets.Answersets);
            foreach (AnswerSet answerSet in answerSets.Answersets)
            {
                Player player = new Player();
                foreach (object obj in answerSet.Atoms)
                {
                    if (typeof(Player).IsInstanceOfType(obj))
                    {
                        player = (Player)obj;
                    }
                    
                    Debug.Log("atom: " + player.Name);
                }
            }



        }
        

        

    }
}