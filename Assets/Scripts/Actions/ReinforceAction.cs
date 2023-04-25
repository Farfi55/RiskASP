using Map;
using player;
using UnityEngine;

namespace Actions
{
    public class ReinforceAction : PlayerAction
    {
        public Territory Territory { get; }
        public int Troops { get; }

        public ReinforceAction(Player player, Territory territory, int troops) : base(player)
        {
            Territory = territory;
            Troops = troops;
            if (IsValid())
                throw new System.ArgumentException("PlaceTroopsAction is not valid");
        }

        private bool IsValid()
        {
            var gm = GameManager.Instance;
            if (gm.CurrentPlayer != Player)
            {
                LogError($"Player ({Player.Name}) is not the current player ({gm.CurrentPlayer.Name})");
                return false;
            }

            if (Territory.Owner != Player)
            {
                LogError($"Territory ({Territory.Name} owned by {Territory.Owner.Name}) is not owned by the player ({Player.Name})");
                return false;
            }

            if (gm.CurrentPhase != gm.ReinforcePhase)
            {
                LogError($"Current phase ({gm.CurrentPhase.Name}) is not ReinforcePhase ({gm.ReinforcePhase.Name})");
            }

            var remainingTroopsToPlace = gm.ReinforcePhase.RemainingTroopsToPlace;
            if (Troops > remainingTroopsToPlace)
            {
                LogError(
                    $"ReinforcePhase: Player ({Player.Name}) tried to place more troops ({Troops}) than they have left ({remainingTroopsToPlace})");
                return false;
            }

            return true;
        }

        private static void LogError(string message)
        {
            Debug.LogError("PlaceTroopsAction: " + message);
        }
    }
}