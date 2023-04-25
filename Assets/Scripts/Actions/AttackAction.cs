using System;
using map;
using player;

namespace Actions
{
    public class AttackAction : AttackPhaseAction
    {
        public Territory From { get; }
        public Territory To { get; }
        public int Troops { get; }
        
        public AttackAction(Player player, Territory from, Territory to, int troops) : base(player)
        {
            From = from;
            To = to;
            Troops = troops;
            if (!IsValid())
                throw new ArgumentException("AttackAction is not valid");
        }

        private bool IsValid()
        {
            var gm = GameManager.Instance;
            if (Player != gm.CurrentPlayer)
            {
                LogError($"Player ({Player.Name}) is not the current player ({gm.CurrentPlayer.Name})");
                return false;
            }
            if(Troops > 3)
            {
                LogError($"Troops ({Troops}) is greater than 3");
                return false;
            }
            if(Troops > From.GetAvailableTroops())
            {
                LogError($"Troops ({Troops}) is greater than available troops ({From.GetAvailableTroops()})");
                return false;
            }
            if(Troops < 1)
            {
                LogError($"Troops ({Troops}) is less than 1");
                return false;
            }
            if(From.Owner != Player)
            {
                LogError($"From territory ({From.Name} owned by {From.Owner.Name}) is not owned by the player ({Player.Name})");
                return false;
            }
            if (To.Owner == Player)
            {
                LogError($"To territory ({To.Name} is a friendly territory)");
                return false;
            }
            if (!From.IsNeighbourOf(To))
            {
                LogError($"From territory ({From.Name}) is not a neighbour of To territory ({To.Name})");
                return false;
            }

            return true;
        }
        
        private static void LogError(string message)
        {
            UnityEngine.Debug.LogError("AttackAction: " + message);
        }
    }
}