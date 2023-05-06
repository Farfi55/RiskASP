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


        public virtual bool IsValid()
        {
            var gm  = GameManager.Instance;
            if(Player != gm.CurrentPlayer)
            {
                LogError($"Player ({Player.Name}) is not the current player ({gm.CurrentPlayer.Name})");
                return false;
            }
            
            if (gm.Turn != Turn)
            {
                LogError($"Turn ({Turn}) is not the current turn ({gm.Turn})");
                return false;
            }
            
            return true;
        }
        
        protected void LogError(string message)
        {
            string name = GetType().Name;
            UnityEngine.Debug.LogError(name + " ERROR: " + message);
        }
    }
}


