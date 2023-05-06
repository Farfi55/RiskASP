using player;

namespace Actions
{
    public abstract class PlayerAction
    {
        public Player Player { get; }
        public int Turn { get; }

        public PlayerAction(Player player, int turn)
        {
            Player = player;
            Turn = turn;
        }
    }
}


