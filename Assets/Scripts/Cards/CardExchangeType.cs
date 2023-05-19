using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using player;

namespace Cards
{
    public class CardExchangeType
    {
        public Dictionary<CardType, int> RequiredCards;
        public int Troops;

        public bool CanExchange(Dictionary<CardType, int> cards)
        {
            foreach (var (cardType, count) in RequiredCards)
            {
                if (!cards.ContainsKey(cardType) || cards[cardType] < count)
                    return false;
            }

            return true;
        }

        public bool CanExchange(IEnumerable<Card> cards)
        {
            var cardsDict = new Dictionary<CardType, int>();
            foreach (var card in cards)
            {
                cardsDict.TryAdd(card.Type, 0);
                cardsDict[card.Type]++;
            }

            return CanExchange(cardsDict);
        }

        
        
        public (List<Card> bestCombination, int bestValue) GetBestExchange(List<Card> cards, player.Player player)
        {
            if (cards.Count < 3)
                return (null, 0);
            
            
            // generate all possible combinations of 3 cards
            List<Card> bestCombination = null;
            int bestValue = 0;
            
            foreach (var cardsCombination in CardsCombinations(cards))
            {
                if (CanExchange(cardsCombination))
                {
                    var exchangeValue = ExchangeValue(cardsCombination, player);
                    if(exchangeValue > bestValue)
                    {
                        bestCombination = cardsCombination;
                        bestValue = exchangeValue;
                    }
                }
            }

            return (bestCombination, bestValue);

        }

        private List<List<Card>> CardsCombinations(List<Card> cards)
        {
            var cardCombinations = new List<List<Card>>();
            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = i + 1; j < cards.Count; j++)
                {
                    for (int k = j + 1; k < cards.Count; k++)
                    {
                        cardCombinations.Add(new List<Card> { cards[i], cards[j], cards[k] });
                    }
                }
            }
            return cardCombinations;
        }

        public int ExchangeValue(List<Card> cards, Player player)
        {
            if (cards.Count != 3)
                throw new ArgumentException("Must exchange exactly 3 cards");
            
            
            int ownedCardTerritories = cards.Count(card => player.HasTerritory(card.Territory));

            return Troops + (2 * ownedCardTerritories);
        }
    }
}