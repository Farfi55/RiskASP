using System;
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
        private readonly ContinentRepository _cr;
        private readonly TerritoryRepository _tr;
        private readonly BattleSimulator _bs;

        public AttackState State => _state;
        private AttackState _state = AttackState.Attacking;
        
        public Action<AttackResult> OnAttacked;
        public Action<AttackReinforceAction> OnReinforced;
        public Action OnAttackStateChanged;


        public AttackPhase(
            GameManager gameManager,
            ContinentRepository continentRepository,
            TerritoryRepository territoryRepository,
            BattleSimulator battleSimulator)
        {
            _gm = gameManager;
            _cr = continentRepository;
            _tr = territoryRepository;
            _bs = battleSimulator;
        }


        public void Start(Player player)
        {
            SetState(AttackState.Attacking);
        }

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

            if (attackResult.HasAttackerWonTerritory())
            {
                attackResult.Target.SetOwner(attackAction.Player, 0);
                SetState(AttackState.Reinforcing);
            }
            

            Debug.Log($"AttackPhase: Attacked {attackResult.Target.Name} from {attackResult.Origin.Name}, " +
                      $"result: {attackResult.AttackerLosses} losses for attacker, " +
                      $"{attackResult.DefenderLosses} losses for defender");
            OnAttacked?.Invoke(attackResult);
        }

        private void HandleAttackReinforceAction(AttackReinforceAction attackReinforceAction)
        {
            if (_state != AttackState.Reinforcing)
                Debug.LogWarning($"AttackPhase: Cannot reinforce while in state {_state}");

            var troops = attackReinforceAction.ReinforcingTroops;
            attackReinforceAction.From.RemoveTroops(troops);
            attackReinforceAction.To.AddTroops(troops);
            
            SetState(AttackState.Attacking);

            Debug.Log(
                $"AttackPhase: Reinforced {troops} troops from {attackReinforceAction.From.Name} to {attackReinforceAction.To.Name}");
            OnReinforced?.Invoke(attackReinforceAction);
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
        Reinforcing,
    }
}