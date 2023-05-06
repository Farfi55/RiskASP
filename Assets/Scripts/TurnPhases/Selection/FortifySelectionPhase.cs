using Actions;
using Map;
using player;
using UnityEngine;

namespace TurnPhases.Selection
{
    public class FortifySelectionPhase : ISelectionPhase
    {
        private readonly GameManager _gm;
        private readonly SelectionManager _sm;
        private readonly ActionReader _ar;
        
        private TerritorySelection _from;
        private TerritorySelection _to;
        
        private int _troopsToMove;

        public FortifySelectionPhase(GameManager gm, SelectionManager sm, ActionReader ar)
        {
            _gm = gm;
            _sm = sm;
            _ar = ar;
        }

        
        public void Start(Player player)
        {
            UnselectAll();
            _troopsToMove = 0;
            EnableTerritoriesToFortifyFrom(player);
        }

        public void OnClicked(Player player, TerritorySelection selection)
        {
            if (!selection.IsSelected)
                OnSelect(player, selection);
            else
                OnUnselect(player, selection);
            
        }

        private void OnSelect(Player player, TerritorySelection selection)
        {
            if(_from == null)
            {
                _from = selection;
                _from.Select();
                
                EnableTerritoriesToFortify(player);
            }
            else if(_to == null)
            {
                _to = selection;
                _to.Select();
                
                EnableTerritoriesToConfirmFortify(player);
            }
        }
        
        private void OnUnselect(Player player, TerritorySelection selection)
        {
            if(selection == _to)
            {
                // todo: add a way to chose number of troops 
                _troopsToMove = _from.Territory.GetAvailableTroops();
                    
                var action = new FortifyAction(player, _gm.Turn, _from.Territory, _to.Territory, _troopsToMove);
                _ar.AddAction(action);
            }
            else
            {
                UnselectAll();
                EnableTerritoriesToFortifyFrom(player);
            }
        }

        private void EnableTerritoriesToFortifyFrom(Player player)
        {
            _sm.DisableAllTerritories();
            _sm.EnablePlayerTerritoriesWithAvailableTroops(player);
        }
        
        private void EnableTerritoriesToConfirmFortify(Player player)
        {
            _sm.DisableAllTerritories();
            _from.Enable();
            _to.Enable();
        }

        private void EnableTerritoriesToFortify(Player player)
        {
            _sm.DisableAllTerritories();
            _sm.EnablePlayerTerritoriesReachableFrom(player, _from);
            _from.Enable();
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