using player;

namespace Actions
{
    public abstract class PlayerAction
    {
        public Player Player { get; }

        public PlayerAction(Player player)
        {
            Player = player;
        }
    }
}


