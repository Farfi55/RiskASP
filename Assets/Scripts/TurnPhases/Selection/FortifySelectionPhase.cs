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
      

        public void Start(Player player)
        {
        }

        public void OnSelected(Player player, TerritorySelection selection)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnselected(Player player, TerritorySelection selection)
        {
            throw new System.NotImplementedException();
        }

        public void End(Player player)
        {
        }
        
    }
}