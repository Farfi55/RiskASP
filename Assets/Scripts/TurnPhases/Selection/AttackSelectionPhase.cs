using System;
using Actions;
using Map;
using player;
using UnityEngine;

namespace TurnPhases.Selection
{
    public class AttackSelectionPhase : ISelectionPhase
    {
        private readonly GameManager _gm;
        private readonly SelectionManager _sm;
        
        private AttackPhase _attackPhase;
        public AttackState State => _attackPhase.State;
        
        
        
        

        public void Start(Player player)
        {
            
        }

        public void OnSelected(Player player, TerritorySelection selection)
        {
            throw new NotImplementedException();
        }

        public void OnUnselected(Player player, TerritorySelection selection)
        {
            throw new NotImplementedException();
        }

        public void End(Player player)
        {
        }

    }
}