using map;
using player;

namespace Turn.Phases
{
    public class FortifyAction : PlayerAction
    {
        public Territory From { get; }
        public Territory To { get; }
        public int MovedTroops { get; }

        public FortifyAction(Player player, Territory from, Territory to, int movedTroops) : base(player)
        {
            From = from;
            To = to;
            MovedTroops = movedTroops;
        }
    }
}