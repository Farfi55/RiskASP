using System.Collections.Generic;
using System.Linq;
using Actions;
using EmbASP.predicates;
using Extensions;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using Map;
using player;

namespace TurnPhases.AI
{
    public class AttackAIPhase : IAIPhase
    {
        private readonly GameManager _gm;
        private readonly ActionReader _ar;
        private readonly TerritoryRepository _tr;


        private AttackPhase _attackPhase => _gm.AttackPhase;
        public AttackState State => _attackPhase.State;


        public AttackAIPhase(GameManager gm, ActionReader ar, TerritoryRepository tr)
        {
            _gm = gm;
            _ar = ar;
            _tr = tr;
        }
        
        public void OnPhaseStart()
        {
        }

        public void OnRequest(player.Player player, InputProgram inputProgram)
        {
            var turn = _gm.Turn;
            var attackTurn = _attackPhase.AttackTurn;

            foreach (var attackResult in _attackPhase.AttackResults)
            {
                var attackAction = attackResult.AttackAction;
                if(attackAction.Turn != turn) 
                    continue;
                
                // TODO: send every attack result to the AI
                //       and a full history of the actions taken so far this phase
                //       when the AI will be able to handle it
                
                var attackResultPredicate = new AttackResultPredicate(attackResult);
                inputProgram.AddObjectInput(attackResultPredicate);
                
                if (attackResult.HasAttackerWonTerritory())
                {
                    var wonTerritory = new WonTerritoryPredicate(turn, attackAction.AttackTurn, player.Name, attackResult.Target.Name);
                    inputProgram.AddObjectInput(wonTerritory);   
                }
            }
            

            var attackTurnPredicate = new AttackTurnPredicate(turn, attackTurn, player.Name);
            inputProgram.AddObjectInput(attackTurnPredicate);
        }

        public void OnResponse(player.Player player, AnswerSet answerSet)
        {
            List<AttackPhaseAction> attackPhaseActions = new List<AttackPhaseAction>();
            EndPhaseAction endPhaseAction = null;

            foreach (var atom in answerSet.Atoms)
            {
                if (atom is EmbASP.predicates.AttackPredicate attack)
                {
                    var action = new AttackAction(player, attack.Turn, attack.AttackTurn,
                        _tr.FromName(attack.From.StripQuotes()), _tr.FromName(attack.To.StripQuotes()), attack.Troops);
                    attackPhaseActions.Add(action);
                }
                else if (atom is EmbASP.predicates.AttackReinforcePredicate afterAttackMove)
                {
                    var attackAction = _attackPhase.LastAttackResult.AttackAction;
                    var action = new AttackReinforceAction(player, afterAttackMove.Turn,
                        afterAttackMove.AttackTurn, attackAction, afterAttackMove.Troops);
                    attackPhaseActions.Add(action);
                }
                else if (atom is EmbASP.predicates.EndAttackPredicate stopAttacking)
                {
                    endPhaseAction = new EndPhaseAction(player, stopAttacking.Turn);
                }
            }

            SortActions(attackPhaseActions);

            foreach (var action in attackPhaseActions) 
                _ar.AddAction(action);
            
            if(endPhaseAction != null)
                _ar.AddAction(endPhaseAction);
        }

        public void OnFailure(Player player)
        {
            if (_attackPhase.State == AttackState.Fortifying)
            {
                var attackAction = _attackPhase.LastAttackResult.AttackAction;
                var troops = _attackPhase.LastAttackResult.GetMinTroopsToMoveAfterWin();
                var action = new AttackReinforceAction(player, _gm.Turn, _attackPhase.AttackTurn, attackAction, troops);
                _ar.AddAction(action);   
            }
            _ar.AddAction(new EndPhaseAction(player, _gm.Turn));
        }

        private void SortActions(List<AttackPhaseAction> actions)
        {
            if (actions.Count > 1)
            {
                actions.Sort((action1, action2) => action1.AttackTurn.CompareTo(action2.AttackTurn));
            }
        }

        public void OnPhaseEnd()
        {
        }
    }
}