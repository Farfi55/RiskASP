using Map;
using player;
using UnityEngine;

namespace Actions
{
    public class ReinforceAction : PlayerAction
    {
        public Territory Territory { get; }
        public int Troops { get; }

        public ReinforceAction(Player player, int turn, Territory territory, int troops) : base(player, turn)
        {
            Territory = territory;
            Troops = troops;
        }

        public override bool IsValid()
        {
            var gm = GameManager.Instance;
            if (gm.CurrentPhase != gm.ReinforcePhase)
            {
                LogError($"Current phase ({gm.CurrentPhase.Name}) is not ReinforcePhase ({gm.ReinforcePhase.Name})");
                return false;
            }

            if (Territory.Owner != Player)
            {
                LogError(
                    $"Territory ({Territory.Name} owned by {Territory.Owner.Name}) is not owned by the player ({Player.Name})");
                return false;
            }

            var remainingTroopsToPlace = gm.ReinforcePhase.RemainingTroopsToPlace;
            if (Troops > remainingTroopsToPlace)
            {
                LogError(
                    $"ReinforcePhase: Player ({Player.Name}) tried to place more troops ({Troops}) than they have left ({remainingTroopsToPlace})");
                return false;
            }

            return base.IsValid();
        }
    }
}