using System;
using Map;
using player;

namespace Actions
{
    public class AttackReinforceAction: AttackPhaseAction
    {
        private AttackAction AttackAction { get; }
        public int ReinforcingTroops { get; }
        public Territory From => AttackAction.Origin;
        public Territory To => AttackAction.Target;
        public int AttackingTroops => AttackAction.Troops;

        public AttackReinforceAction(Player player, AttackAction attackAction, int reinforcingTroops) : base(player)
        {
            AttackAction = attackAction;
            ReinforcingTroops = reinforcingTroops;
            
            if(!IsValid())
                throw new ArgumentException("AttackReinforceAction is not valid");
        }

        private bool IsValid()
        {
            if(Player != AttackAction.Player)
            {
                LogError($"Player ({Player.Name}) is not the same as the player in the attack action ({AttackAction.Player.Name})");
                return false;
            }
            if(ReinforcingTroops > From.GetAvailableTroops())
            {
                LogError($"Reinforcing troops ({ReinforcingTroops}) is greater than available troops ({From.GetAvailableTroops()})");
                return false;
            }

            var minReinforcing = Math.Min(AttackingTroops, From.GetAvailableTroops());
            if(ReinforcingTroops < minReinforcing)
            {
                LogError($"Reinforcing troops ({ReinforcingTroops}) is less than the minimum ({minReinforcing})");
                return false;
            }
            return true;
        }
        
        private static void LogError(string message)
        {
            UnityEngine.Debug.LogError($"AttackReinforceAction: {message}");
        }
    }
}