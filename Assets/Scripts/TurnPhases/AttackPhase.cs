using System;
using System.Collections.Generic;
using System.Linq;
using Actions;
using JetBrains.Annotations;
using Map;
using player;
using UnityEngine;

namespace TurnPhases
{
    public class AttackPhase : IPhase
    {
        public string Name => "Attack";

        private readonly GameManager _gm;
        private readonly BattleSimulator _bs;

        public int ConqueredTerritoriesCount { get; private set; }

        public AttackState State => _state;
        private AttackState _state = AttackState.Attacking;

        public int AttackTurn => _attackTurn;
        private int _attackTurn;


        private List<AttackResult> _attackResults = new ();
        public AttackResult LastAttackResult => _attackResults.Last();
        public IEnumerable<AttackResult> AttackResults => _attackResults;

        public Action<AttackResult> OnAttacked;
        public Action<AttackReinforceAction> OnReinforced;
        public Action OnAttackStateChanged;
        public Action OnAttackTurn;

        public AttackPhase(GameManager gm, BattleSimulator bs)
        {
            _gm = gm;
            _bs = bs;
        }


        public void Start(Player player)
        {
            _attackResults.Clear();
            ConqueredTerritoriesCount = 0;
            _attackTurn = 1;
            SetState(AttackState.Attacking);
            OnAttackTurn?.Invoke();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void OnAction(Player player, PlayerAction action)
        {
            if (action is AttackAction attackAction)
                HandleAttackAction(attackAction);
            else if (action is AttackReinforceAction attackReinforceAction)
                HandleAttackReinforceAction(attackReinforceAction);
            else if (action is EndPhaseAction)
            {
                if (_state == AttackState.Attacking)
                    _gm.NextTurnPhase();
                else
                    Debug.LogWarning($"AttackPhase: Cannot end Attack phase while in state {_state}");
            }
            else
                Debug.LogError($"AttackPhase: Received action of type {action.GetType().Name}");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void HandleAttackAction(AttackAction attackAction)
        {
            if (_state != AttackState.Attacking)
            {
                Debug.LogWarning($"AttackPhase: Cannot attack while in state {_state}");
                return;
            }

            var attackResult = _bs.SimulateAttack(attackAction);

            attackResult.Origin.RemoveTroops(attackResult.AttackerLosses);
            attackResult.Target.RemoveTroops(attackResult.DefenderLosses);
            _attackResults.Add(attackResult);
            _attackTurn++;

            
            if (attackResult.HasAttackerWonTerritory())
            {
                attackResult.Target.SetOwner(attackAction.Player, 0);
                ConqueredTerritoriesCount++;
                SetState(AttackState.Fortifying);
            }
            
           
            // Debug.Log($"AttackPhase: Attacked {attackResult.Target.Name} from {attackResult.Origin.Name}, " +
            //           $"result: {attackResult.AttackerLosses} losses for attacker, " +
            //           $"{attackResult.DefenderLosses} losses for defender");
            

            OnAttacked?.Invoke(attackResult);
            OnAttackTurn?.Invoke();
        }

        private void HandleAttackReinforceAction(AttackReinforceAction attackReinforceAction)
        {
            if (_state != AttackState.Fortifying)
                Debug.LogWarning($"AttackPhase: Cannot reinforce while in state {_state}");

            var troops = attackReinforceAction.ReinforcingTroops;
            attackReinforceAction.From.RemoveTroops(troops);
            attackReinforceAction.To.AddTroops(troops);
            _attackTurn++;
            
            SetState(AttackState.Attacking);

            Debug.Log(
                $"AttackPhase: Reinforced {troops} troops from {attackReinforceAction.From.Name} to {attackReinforceAction.To.Name}");
            
            OnReinforced?.Invoke(attackReinforceAction);
            OnAttackTurn?.Invoke();
        }

        public void End(Player player)
        {
        }
        
        private void SetState(AttackState state)
        {
            _state = state;
            OnAttackStateChanged?.Invoke();
        }
        
    }

    public enum AttackState
    {
        Attacking,
        Fortifying,
    }
}