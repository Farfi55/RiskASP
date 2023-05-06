using System;
using Map;
using player;
using TurnPhases;

namespace Actions
{
    public class AttackAction : AttackPhaseAction
    {
        public Territory Origin { get; }
        public Territory Target { get; }
        public int Troops { get; }

        public AttackAction(Player player, int turn, int attackTurn, Territory origin, Territory target, int troops) :
            base(player, turn, attackTurn)
        {
            Origin = origin;
            Target = target;
            Troops = troops;
        }

        public override bool IsValid()
        {
            var gm = GameManager.Instance;
            if(gm.AttackPhase.State != AttackState.Attacking)
            {
                LogError($"AttackPhase is not in Attacking state");
                return false;
            }
            
            if (Troops > 3)
            {
                LogError($"Troops ({Troops}) is greater than 3");
                return false;
            }

            if (Troops > Origin.GetAvailableTroops())
            {
                LogError($"Troops ({Troops}) is greater than available troops ({Origin.GetAvailableTroops()})");
                return false;
            }

            if (Troops < 1)
            {
                LogError($"Troops ({Troops}) is less than 1");
                return false;
            }

            if (Origin.Owner != Player)
            {
                LogError(
                    $"Origin territory ({Origin.Name} owned by {Origin.Owner.Name}) is not owned by the player ({Player.Name})");
                return false;
            }

            if (Target.Owner == Player)
            {
                LogError($"Target territory ({Target.Name} is a friendly territory)");
                return false;
            }

            if (!Origin.IsNeighbourOf(Target))
            {
                LogError($"Origin territory ({Origin.Name}) is not a neighbour of Target territory ({Target.Name})");
                return false;
            }

            return base.IsValid();
        }

    }
}