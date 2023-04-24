using System;
using map;
using player;
using Unity.VisualScripting;
using UnityEngine;

namespace Turn.Phases
{
    public class FortifyPhase : IPhase
    {
        private readonly GameManager _gm;
        private readonly ContinentRepository _cr;
        private readonly TerritoryRepository _tr;

        public Action OnTroopsToPlaceChanged;

        public FortifyPhase(GameManager gameManager, ContinentRepository continentRepository,
            TerritoryRepository territoryRepository)
        {
            _gm = gameManager;
            _cr = continentRepository;
            _tr = territoryRepository;
        }

        public void Start(Player player)
        {
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
                OnTroopsToPlaceChanged?.Invoke();
            }
            else
                Debug.LogWarning($"FortifyPhase: Received action of type {action.GetType().Name}");
        }


        public void End(Player player)
        {
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
                Debug.LogError(
                    $"FortifyAction: From territory ({action.From.Name}) cannot reach to territory ({action.To.Name})");
            }
            else if (action.MovedTroops <= 0)
            {
                Debug.LogError(
                    $"FortifyAction: Moved troops ({action.MovedTroops}) is less than or equal to 0");
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