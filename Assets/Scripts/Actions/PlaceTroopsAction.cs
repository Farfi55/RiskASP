using map;
using player;

namespace Turn.Phases
{
    public class PlaceTroopsAction : PlayerAction
    {
        public Territory Territory { get; }
        public int Troops { get; }
        
        public PlaceTroopsAction(Player player, Territory territory, int troops) : base(player)
        {
            Territory = territory;
            Troops = troops;
        }
    }
}