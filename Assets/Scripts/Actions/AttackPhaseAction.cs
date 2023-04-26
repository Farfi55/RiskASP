using player;

namespace Actions
{
    public abstract class AttackPhaseAction : PlayerAction
    {
        protected AttackPhaseAction(Player player) : base(player)
        {
        }
    }
}