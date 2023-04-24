using map;
using player;
using UnityEngine;

namespace Turn.Phases
{
    public class ReinforcePhase : IPhase
    {
        private readonly GameManager _gm;
        private readonly ContinentRepository _cr;
        private readonly TerritoryRepository _tr;
        private int _remainingTroopsToPlace;

        public ReinforcePhase(GameManager gameManager, ContinentRepository continentRepository,
            TerritoryRepository territoryRepository)
        {
            _gm = gameManager;
            _cr = continentRepository;
            _tr = territoryRepository;
        }

        public void Start(Player player)
        {
            _remainingTroopsToPlace = player.GetTotalTroopBonus();
            if (_remainingTroopsToPlace == 0) _gm.NextTurnPhase();
        }

        public void OnAction(Player player, PlayerAction action)
        {
            if (action is PlaceTroopsAction placeTroopsAction)
            {
                if (CheckActionValidity(player, placeTroopsAction))
                    return;

                placeTroopsAction.Territory.AddTroops(placeTroopsAction.Troops);
                _remainingTroopsToPlace -= placeTroopsAction.Troops;
            }
            else
                Debug.LogError($"ReinforcePhase: Received action of type {action.GetType().Name}");

            if (_remainingTroopsToPlace == 0)
                _gm.NextTurnPhase();
        }


        private bool CheckActionValidity(Player player, PlaceTroopsAction action)
        {
            if (action == null)
                Debug.LogError($"ReinforcePhase: Received null action");
            else if (player != action.Player)
                Debug.LogError($"ReinforcePhase: Player ({action.Player.Name}) is not the current player ({player.Name})");
            else if (action.Territory.Owner != player)
                Debug.LogError(
                    $"ReinforcePhase: Territory ({action.Territory.Name} owned by {action.Territory.Owner.Name}) is not owned by the player ({player.Name})");
            else if (action.Troops > _remainingTroopsToPlace)
                Debug.LogError(
                    $"ReinforcePhase: Player ({player.Name}) tried to place more troops ({action.Troops}) than they have left ({_remainingTroopsToPlace})");
            else
                return true;

            return false;
        }

        public void End(Player player)
        {
            
        }
    }
}