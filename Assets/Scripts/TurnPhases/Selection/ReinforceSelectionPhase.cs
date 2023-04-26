using System;
using Actions;
using Map;
using player;
using UnityEngine;

namespace TurnPhases.Selection
{
    public class ReinforceSelectionPhase : ISelectionPhase
    {
        private readonly GameManager _gm;
        private readonly SelectionManager _sm;
        private readonly ActionReader _ar;
        
        public TerritorySelection SelectedTerritory => _selectedTerritory;
        private TerritorySelection _selectedTerritory;

        private ReinforcePhase _reinforcePhase => _gm.ReinforcePhase;
        

        public ReinforceSelectionPhase(GameManager gm, SelectionManager sm, ActionReader ar)
        {
            _gm = gm;
            _sm = sm;
            _ar = ar;
        }


        public void Start(Player player)
        {
            _sm.DisableAllTerritories();
            _sm.EnablePlayerTerritories(player);
            _selectedTerritory = null;
        }

        public void OnClicked(Player player, TerritorySelection selection)
        {
            if (_selectedTerritory != null)
            {
                Unselect();
                return;
            }
            
            _selectedTerritory = selection;
            _selectedTerritory.Select();
            
            // TODO: add a way to select how many troops to place
            // var troopsToPlace = _reinforcePhase.RemainingTroopsToPlace;
            var action = new ReinforceAction(player, selection.Territory, 1);
            _ar.AddAction(action);
            Unselect();
        }
        
        public void End(Player player)
        {
            Unselect();
        }

        private void Unselect()
        {
            if (_selectedTerritory != null)
                _selectedTerritory.Unselect();
            _selectedTerritory = null;
        }
    }
}