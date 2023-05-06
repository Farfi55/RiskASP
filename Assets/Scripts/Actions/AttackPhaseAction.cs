using player;

namespace Actions
{
    public abstract class AttackPhaseAction : PlayerAction
    {
        public int AttackTurn { get; }
        
        protected AttackPhaseAction(Player player, int turn, int attackTurn) : base(player, turn)
        {
            AttackTurn = attackTurn;
        }
        
        public override bool IsValid()
        {
            var gm = GameManager.Instance;
            
            if (AttackTurn != gm.AttackPhase.AttackTurn)
            {
                LogError($"Attack turn ({AttackTurn}) is not the current attack turn ({gm.AttackPhase.AttackTurn})");
                return false;
            }
            
            if (gm.CurrentPhase != gm.AttackPhase)
            {
                LogError($"Current phase ({gm.CurrentPhase.Name}) is not AttackPhase ({gm.AttackPhase.Name})");
                return false;
            }

            return base.IsValid();
        }
    }
}