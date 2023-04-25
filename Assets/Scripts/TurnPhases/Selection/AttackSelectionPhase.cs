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

        private AttackResult _lastAttackResult;

        public AttackSelectionPhase(GameManager gm, SelectionManager sm, ActionReader ar)
        {
            _gm = gm;
            _sm = sm;
            _ar = ar;
            _attackPhase.OnAttacked += OnAttacked;
            _attackPhase.OnAttackStateChanged += OnAttackStateChanged;
        }

        private void OnAttacked(AttackResult result) => _lastAttackResult = result;
        
        private void OnAttackStateChanged()
        {
            
        }



        public void Start(Player player)
        {
            _from = null;
            _to = null;
            _lastAttackResult = null;
            
            _sm.EnablePlayerTerritoriesWithAttackPossibility(player);
        }

        public void OnSelected(Player player, TerritorySelection selection)
        {
            if (selection == null) throw new ArgumentNullException(selection.Territory.Name);
            
            if (_from == null)
            {
                _from = selection;
                _sm.DisableAllTerritories();
                _from.Enable();
                _sm.EnableEnemyNeighbourTerritories(_from.Territory);
            }
            else if (_to == null)
            {
                if(_from == selection) 
                    throw new Exception("Cannot attack from a territory to itself");
                _to = selection;

                if (State == AttackState.Attacking)
                {
                    var troops = Math.Min(_from.Territory.GetAvailableTroops(), 3);
                    var action = new AttackAction(player, _from.Territory, _to.Territory, troops);
                    _ar.AddAction(action);
                }
                else if (State == AttackState.Reinforcing)
                {
                    var troops = _from.Territory.GetAvailableTroops();
                    var attackAction = _lastAttackResult.AttackAction;
                    var action = new AttackReinforceAction(player, attackAction, troops);
                    _ar.AddAction(action);
                }
            }
        }

        public void OnUnselected(Player player, TerritorySelection selection)
        {
            if(_from == selection)
                _from = null;
            else if(_to == selection)
                _to = null;
            
        }

        public void End(Player player)
        {
            UnselectAll();
        }
        

        private void UnselectAll()
        {
            if(_from) _from.Unselect();
            if(_to) _to.Unselect();
            _from = null;
            _to = null;
                
        }
        
    }
}