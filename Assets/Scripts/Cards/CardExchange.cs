using System;
using player;

namespace Cards
{
    public class CardExchange
    {
        public Card[] Cards { get; }
        public CardExchangeType ExchangeType { get; }
        
        public Player Player { get; }
        public int ExchangeValue { get; }
        
        public CardExchange(Card[] cards, CardExchangeType exchangeType, Player player, int exchangeValue)
        {
            Cards = cards;
            ExchangeType = exchangeType;
            Player = player;
            ExchangeValue = exchangeValue;
            
            if(cards.Length != 3)
                throw new ArgumentException($"Cards length must be 3, but was {cards.Length}");
            
            if(!exchangeType.CanExchange(cards))
                throw new ArgumentException($"Cards {cards} cannot be exchanged for {exchangeType}");

            var value = exchangeType.ExchangeValue(cards, player);
            if(exchangeValue != value)
                throw new ArgumentException($"Exchange value ({exchangeValue}) is not the correct value ({value})");
                
        }
        
    }
}