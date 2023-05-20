using System.Linq;
using Cards;
using player;

namespace Actions
{
    class ExchangeCardsAction : PlayerAction
    {
        public Card[] Cards { get; }
        public CardExchangeType ExchangeType { get; }
        public int ExchangeValue { get; }


        public ExchangeCardsAction(Player player, int turn, Card[] cards, CardExchangeType exchangeType, int exchangeValue) : base(player, turn)
        {
            Cards = cards;
            ExchangeType = exchangeType;
            ExchangeValue = exchangeValue;
        }


        public override bool IsValid()
        {
            GameManager gm = GameManager.Instance;
            
            if (gm.CurrentPhase != gm.ReinforcePhase)
            {
                LogError($"Current phase ({gm.CurrentPhase.Name}) is not ReinforcePhase");
                return false;
            }
            
            if(Cards.Distinct() != Cards)
            {
                LogError($"Cards ({Cards}) contains duplicates");
                return false;
            }
            
            if (Cards.Length != 3)
            {
                LogError($"Cards ({Cards}) length is not 3");
                return false;
            }

            if(!ExchangeType.CanExchange(Cards))
            {
                LogError($"Cards ({Cards}) cannot be exchanged for {ExchangeType}");
                return false;
            }

            var value = ExchangeType.ExchangeValue(Cards, Player);
            if (value != ExchangeValue)
            {
                LogError($"Exchange value ({ExchangeValue}) is not the correct value ({value})");
                return false;
            }
            
            return base.IsValid();
        }
    }
}