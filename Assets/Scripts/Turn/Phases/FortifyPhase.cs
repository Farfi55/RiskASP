using System;
using map;
using player;
using Unity.VisualScripting;
using UnityEngine;

namespace Turn.Phases
{
    class FortifyPhase : IPhase
    {
        private GameManager _gm;
        private ContinentRepository _cr;
        private TerritoryRepository _tr;

        public int _troopsToPlace { get; private set; } = 0;
        public Action OnTroopsToPlaceChanged;

        public void Setup(GameManager gameManager, ContinentRepository continentRepository,
            TerritoryRepository territoryRepository)
        {
            _gm = gameManager;
            _cr = continentRepository;
            _tr = territoryRepository;
        }

        public void Start(Player player)
        {
            _troopsToPlace = player.GetTotalTroopBonus();
            OnTroopsToPlaceChanged?.Invoke();
        }

        public void OnAction(Player player, PlayerAction action)
        {
            if (action is FortifyAction fortifyAction)
            {
                if (CheckActionValidity(player, fortifyAction))
                    return;

                fortifyAction.From.RemoveTroops(fortifyAction.MovedTroops);
                fortifyAction.To.AddTroops(fortifyAction.MovedTroops);
                _troopsToPlace -= fortifyAction.MovedTroops;

                OnTroopsToPlaceChanged?.Invoke();
            }
        }


        public void End(Player player)
        {
            throw new NotImplementedException();
        }
        
        
        private bool CheckActionValidity(Player player, FortifyAction action)
        {
            if (player != action.Player)
                Debug.LogError(
                    $"FortifyAction: Player ({action.Player.Name}) is not the current player ({player.Name})");
            else if (action.From.Owner == action.Player)
                Debug.LogError(
                    $"FortifyAction: From territory ({action.From.Name} owned by {action.From.Owner.Name}) is not owned by the player ({player.Name})");
            else if (action.To.Owner == action.Player)
                Debug.LogError(
                    $"FortifyAction: To territory ({action.To.Name} owned by {action.To.Owner.Name}) is not owned by the player ({player.Name})");
            else if (action.MovedTroops > action.From.GetAvailableTroops())
                Debug.LogError(
                    $"FortifyAction: Moved troops ({action.MovedTroops}) is greater than available troops ({action.From.GetAvailableTroops()})");
            else if (!_tr.CanReachTerritory(action.From, action.To))
            {
                
            }
            else
            {
                // nothing went wrong
                return true;
            }

            // something went wrong
            return false;
        }
        
    }
}