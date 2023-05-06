using System;
using Map;
using player;
using TurnPhases;

namespace Actions
{
    public class AttackReinforceAction : AttackPhaseAction
    {
        private AttackAction AttackAction { get; }
        public int ReinforcingTroops { get; }
        public Territory From => AttackAction.Origin;
        public Territory To => AttackAction.Target;
        public int AttackingTroops => AttackAction.Troops;

        public AttackReinforceAction(Player player, int turn, int attackTurn, AttackAction attackAction,
            int reinforcingTroops) : base(player, turn, attackTurn)
        {
            AttackAction = attackAction;
            ReinforcingTroops = reinforcingTroops;
        }

        public override bool IsValid()
        {
            var gm = GameManager.Instance;
            if(gm.AttackPhase.State != AttackState.Fortifying)
            {
                LogError("AttackPhase is not in Fortifying state");
                return false;
            }
            
            if (Player != AttackAction.Player)
            {
                LogError(
                    $"Player ({Player.Name}) is not the same as the player in the attack action ({AttackAction.Player.Name})");
                return false;
            }

            if (ReinforcingTroops > From.GetAvailableTroops())
            {
                LogError(
                    $"Reinforcing troops ({ReinforcingTroops}) is greater than available troops ({From.GetAvailableTroops()})");
                return false;
            }

            var minReinforcing = Math.Min(Math.Min(AttackingTroops, 3), From.GetAvailableTroops());
            if (ReinforcingTroops < minReinforcing)
            {
                LogError($"Reinforcing troops ({ReinforcingTroops}) is less than the minimum ({minReinforcing})");
                return false;
            }

            return base.IsValid();
        }
    }
}