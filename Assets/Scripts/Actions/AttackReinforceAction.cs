using System;
using map;
using player;

namespace Actions
{
    class AttackReinforceAction: AttackPhaseAction
    {
        private AttackAction AttackAction { get; }
        public int ReinforcingTroops { get; }
        public Territory From => AttackAction.From;
        public Territory To => AttackAction.To;
        public int AttackingTroops => AttackAction.Troops;

        public AttackReinforceAction(Player player, AttackAction attackAction, int reinforcingTroops) : base(player)
        {
            AttackAction = attackAction;
            ReinforcingTroops = reinforcingTroops;
            if(!IsValid())
                throw new System.ArgumentException("AttackReinforceAction is not valid");
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