using System.Linq;
using Cards;
using player;

namespace Actions
{
    class ExchangeCardsAction : PlayerAction
    {
        public CardExchange Exchange { get; }


        public ExchangeCardsAction(Player player, int turn, CardExchange exchange) : base(player, turn)
        {
            Exchange = exchange;
        }


        public override bool IsValid()
        {
            GameManager gm = GameManager.Instance;
            
            if (gm.CurrentPhase != gm.ReinforcePhase)
            {
                LogError($"Current phase ({gm.CurrentPhase.Name}) is not ReinforcePhase");
                return false;
            }
            
            return base.IsValid();
        }
    }
}