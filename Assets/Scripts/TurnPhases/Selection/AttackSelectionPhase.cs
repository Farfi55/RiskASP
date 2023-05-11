using System;
using Actions;
using Map;
using player;
using Unity.VisualScripting;
using UnityEngine;

namespace TurnPhases.Selection
{
    public class AttackSelectionPhase : ISelectionPhase
    {
        private readonly GameManager _gm;
        private readonly SelectionManager _sm;
        private readonly ActionReader _ar;

        private AttackPhase _attackPhase => _gm.AttackPhase;
        public AttackState State => _attackPhase.State;

        private TerritorySelection _from;
        private TerritorySelection _to;

        private int _reinforcementsToMove;

        public AttackSelectionPhase(GameManager gm, SelectionManager sm, ActionReader ar)
        {
            _gm = gm;
            _sm = sm;
            _ar = ar;
            _attackPhase.OnAttackTurn += OnAttackTurn;
        }

        private void OnAttackTurn()
        {
            if (State == AttackState.Attacking)
            {
                UnselectAll();
                EnableTerritoriesToAttackFrom(_gm.CurrentPlayer);
            }
            else if (State == AttackState.Fortifying)
            {
                if (_from == null)
                {
                    var from = _attackPhase.LastAttackResult.Origin;
                    _from = _sm.TerritoryToSelectionMap[from];
                    Debug.Log($"set from: {from.Name}");
                }
                if (_to == null)
                {
                    var to = _attackPhase.LastAttackResult.Target;
                    _to = _sm.TerritoryToSelectionMap[to];
                    Debug.Log($"set to: {to.Name}");
                }

                EnableTerritoriesToConfirmMove();
            }
        }


        public void Start(Player player)
        {
            UnselectAll();
            _reinforcementsToMove = 0;
            EnableTerritoriesToAttackFrom(player);
        }

        public void OnClicked(Player player, TerritorySelection selection)
        {
            if (selection == null) throw new ArgumentNullException(selection.Territory.Name);

            if (!selection.IsSelected) OnSelected(player, selection);
            else OnUnselected(player, selection);
        }

        private void OnSelected(Player player, TerritorySelection selection)
        {
            if (State == AttackState.Fortifying)
                return;

            if (_from == null)
            {
                _from = selection;
                _from.Select();
                EnableTerritoriesToAttack(player);
            }
            else if (_to == null)
            {
                if (_from == selection)
                    throw new Exception("Cannot attack from a territory to itself");
                _to = selection;
                _to.Select();

                CreateAttackAction(player);
            }
        }

        private void CreateAttackAction(Player player)
        {
            var troops = Math.Min(_from.Territory.GetAvailableTroops(), 3);
            var attackTurn = _attackPhase.AttackTurn;
            var action = new AttackAction(player, _gm.Turn, attackTurn, _from.Territory, _to.Territory, troops);
            _ar.AddAction(action);
        }

        public void OnUnselected(Player player, TerritorySelection selection)
        {
            if (State == AttackState.Attacking)
                OnUnselectedAttacking(player, selection);
            else if (State == AttackState.Fortifying)
                OnUnselectedMoving(player, selection);
        }


        private void OnUnselectedAttacking(Player player, TerritorySelection selection)
        {
            if (_from == selection)
            {
                UnselectAll();
                EnableTerritoriesToAttackFrom(player);
            }
            else if (_to == selection)
            {
                _to.Unselect();
                _to = null;
                EnableTerritoriesToAttack(player);
            }
            else Debug.LogError($"Territory {selection.Territory.Name} is not from or to");
        }

        private void OnUnselectedMoving(Player player, TerritorySelection selection)
        {
            if (_from == selection)
            {
                SetTroopsToMove(_attackPhase.LastAttackResult.GetMinTroopsToMoveAfterWin());
                CreateMoveAction(player);
            }
            else if (_to == selection)
            {
                SetTroopsToMove(_from.Territory.GetAvailableTroops());
                CreateMoveAction(player);
            }
            else Debug.LogError($"Territory {selection.Territory.Name} is not from or to");
        }

        private void CreateMoveAction(Player player)
        {
            var attackAction = _attackPhase.LastAttackResult.AttackAction;
            var attackTurn = _attackPhase.AttackTurn;
            var action = new AttackReinforceAction(player, _gm.Turn, attackTurn, attackAction, _reinforcementsToMove);
            _ar.AddAction(action);
        }

        public void End(Player player)
        {
            UnselectAll();
        }


        private void EnableTerritoriesToAttackFrom(Player player)
        {
            _sm.DisableAllTerritories();
            _sm.EnablePlayerTerritoriesWithAttackPossibility(player);
        }

        private void EnableTerritoriesToAttack(Player player)
        {
            _sm.DisableAllTerritories();
            _sm.EnableEnemyNeighbourTerritories(_from.Territory);
            _sm.EnableTerritory(_from);
        }


        private void EnableTerritoriesToConfirmMove()
        {
            _sm.DisableAllTerritories();
            _sm.EnableTerritories(_from, _to);
        }

        private void UnselectAll()
        {
            if (_from) _from.Unselect();
            if (_to) _to.Unselect();
            _from = null;
            _to = null;
        }

        public void SetTroopsToMove(int troops)
        {
            if (State == AttackState.Fortifying)
                _reinforcementsToMove = troops;
            else
                Debug.LogError("Cannot set troops to move when not in moving state");
        }
    }
}