using System;
using Actions;
using EmbASP;
using EmbASP.predicates;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using Map;
using UnityEngine;
using Player = player.Player;

namespace TurnPhases.AI
{
    public class AttackAIPhase : IAIPhase
    {
        private readonly GameManager _gm;
        private readonly ActionReader _ar;
        private readonly TerritoryRepository _tr;
        private readonly Player _pl;
        private int _attackTurn;
        
        

        private AttackPhase _attackPhase => _gm.AttackPhase;
        public AttackState State => _attackPhase.State;

        
        public AttackAIPhase(GameManager gm, ActionReader ar, TerritoryRepository tr)
        {
            _gm = gm;
            _ar = ar;
            _tr = tr;
        }
        
        public void Start(InputProgram inputProgram)
        {
            if (_attackPhase.LastAttackResult.HasAttackerWonTerritory())
            {
                //
            }
            inputProgram.AddObjectInput(new AttackTurn(_gm.Turn,_attackPhase.AttackTurn,_gm.CurrentPlayer.Name));
            foreach (var territory in _tr.Territories.Values)
            {
                inputProgram.AddObjectInput(new TerritoryControl(_gm.Turn,territory.Name,territory.Owner.Name,territory.Troops));
            }
        }

        public void OnResponse(AnswerSet answerSet)
        {
            try
            {
                foreach (var obj in answerSet.Atoms)
                {
                    if (obj is StopAttacking)
                    {
                        _ar.AddAction(new EndPhaseAction(_gm.CurrentPlayer,_gm.Turn));
                    }
                    
                    else if(obj is Attack)
                    {
                        var attack = (Attack) obj;
                        var attackAction = new AttackAction(_gm.CurrentPlayer,_gm.Turn,attack.AttackTurn,_tr.Territories[attack.From], _tr.Territories[attack.To], attack.Armies);
                        _ar.AddAction(attackAction);
                    }
                    
                    
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public void End(Player player)
        {
            throw new NotImplementedException();
        }
    }
}