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
    }
}