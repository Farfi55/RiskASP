using System;
using Actions;
using EmbASP;
using EmbASP.predicates;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using Map;
using UnityEngine;

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

        public void OnRequest(player.Player player, InputProgram inputProgram)
        {
            var turn = _gm.Turn;
            var attackTurn = _attackPhase.AttackTurn;
            var lastAttackAction = _attackPhase.LastAttackResult.AttackAction;
            if (lastAttackAction.Turn == turn && lastAttackAction.AttackTurn == attackTurn - 1)
            {
                var attackResult = new EmbASP.predicates.AttackResultPredicate(_attackPhase.LastAttackResult);
                inputProgram.AddObjectInput(attackResult);
            }

            var attackTurnPredicate = new AttackTurnPredicate(turn, attackTurn, player.Name);
            inputProgram.AddObjectInput(attackTurnPredicate);
        }

        public void OnResponse(player.Player player, AnswerSet answerSet)
        {
            foreach (var atom in answerSet.Atoms)
            {
                PlayerAction action = null;
                if (atom is EmbASP.predicates.AttackPredicate attack)
                {
                    action = new AttackAction(player, attack.Turn, attack.AttackTurn,
                        _tr.FromName(attack.From), _tr.FromName(attack.To), attack.Troops);
                }
                else if (atom is EmbASP.predicates.AttackReinforcePredicate afterAttackMove)
                {
                    var attackAction = _attackPhase.LastAttackResult.AttackAction;
                    action = new AttackReinforceAction(player, afterAttackMove.Turn,
                        afterAttackMove.AttackTurn, attackAction, afterAttackMove.Troops);
                }
                else if (atom is EmbASP.predicates.EndAttackPredicate stopAttacking)
                {
                    action = new EndPhaseAction(player, stopAttacking.Turn);
                }

                if (action != null)
                    _ar.AddAction(action);
            }
        }

        public void End(player.Player player)
        {
            throw new NotImplementedException();
        }
    }
}