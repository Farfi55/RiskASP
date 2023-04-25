using Actions;
using map;
using player;
using UnityEngine;

namespace Turn.Phases
{
    public class AttackPhase : IPhase
    {
        public string Name => "Attack";

        private readonly GameManager _gm;
        private readonly ContinentRepository _cr;
        private readonly TerritoryRepository _tr;
        private readonly BattleSimulator _bs;

        private AttackState _state = AttackState.Attack;


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
            _gm.NextTurnPhase(); // TODO: remove this line
        }

        public void OnAction(Player player, PlayerAction action)
        {
            if (action is AttackAction attackAction)
                HandleAttackAction(attackAction);
            else if (action is AttackReinforceAction attackReinforceAction)
                HandleAttackReinforceAction(attackReinforceAction);
            else if (action is EndPhaseAction)
            {
                if (_state == AttackState.Attack)
                    _gm.NextTurnPhase();
                else
                    Debug.LogWarning($"AttackPhase: Cannot end Attack phase while in state {_state}");
            }
            else
                Debug.LogError($"AttackPhase: Received action of type {action.GetType().Name}");
        }

        private void HandleAttackAction(AttackAction attackAction)
        {
        }

        private void HandleAttackReinforceAction(AttackReinforceAction attackReinforceAction)
        {
            throw new System.NotImplementedException();
        }

        public void End(Player player)
        {
            // TODO: implement
        }
    }

    internal enum AttackState
    {
        Attack,
        Reinforce,
    }
}