using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using JetBrains.Annotations;
using Map;
using player;
using UnityEngine;

namespace Cards
{
    public class CardRepository : MonoBehaviour
    {
        public static CardRepository Instance { get; private set; }
        
        private TerritoryRepository _tr;

        public Card[] AllCards { get; private set; }

        public HashSet<Card> CardsInDeck { get; private set; }

        public CardExchangeType[] ExchangeTypes;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("More than one TerritoryRepository in scene");
                Destroy(gameObject);
            }
            else
                Instance = this;
            
            _tr = TerritoryRepository.Instance;

            AllCards = GenerateCards();
            CardsInDeck = new HashSet<Card>(AllCards);
            foreach (var card in AllCards) CardsInDeck.Add(card);
        }
        

        private Card[] GenerateCards()
        {
            // 2 wild
            // 14 infantry
            // 14 cavalry
            // 14 artillery
            Card[] cards = new Card[44];

            var territories = _tr.Territories.ToArray();
            territories.Shuffle();

            for (int i = 0; i < 14; i++)
            {
                cards[i] = new Card(CardType.Infantry, territories[i]);
                cards[i + 14] = new Card(CardType.Cavalry, territories[i + 14]);
                cards[i + 28] = new Card(CardType.Artillery, territories[i + 28]);
            }

            cards[42] = new Card(CardType.Wild, null);
            cards[43] = new Card(CardType.Wild, null);
            cards.Shuffle();
            return cards;
        }

        private IEnumerable<CardExchangeType> GenerateExchangeTypes()
        {
            return new CardExchangeType[]
            {
                new(requiredCards: new() { { CardType.Artillery, 3 } }, troops: 4),
                new(requiredCards: new() { { CardType.Infantry, 3 } }, troops: 6),
                new(requiredCards: new() { { CardType.Cavalry, 3 } }, troops: 8),
                new(requiredCards: new() { { CardType.Wild, 1 }, { CardType.Artillery, 2 } }, troops: 12),
                new(requiredCards: new() { { CardType.Wild, 1 }, { CardType.Infantry, 2 } }, troops: 12),
                new(requiredCards: new() { { CardType.Wild, 1 }, { CardType.Cavalry, 2 } }, troops: 12)
            };
        }

        public Card DrawRandomCard()
        {
            if (CardsInDeck.Count == 0)
                return null;

            var card = CardsInDeck.ElementAt(UnityEngine.Random.Range(0, CardsInDeck.Count));
            CardsInDeck.Remove(card);
            return card;
        }

        public void ReturnCardsToDeck(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
                CardsInDeck.Add(card);
        }
        
        
        public List<(List<Card> combination, int value)> GetBestExchanges(player.Player player)
        {
            var bestExchanges = new List<(List<Card> combination, int value)>();
            var cards = player.Cards;

            foreach (var cardExchangeType in ExchangeTypes)
            {
                var (combination, value) = cardExchangeType.GetBestExchange(cards, player);
                if (combination != null && value > 0)
                    bestExchanges.Add((combination, value));
            }

            return bestExchanges;
        }

        [CanBeNull]
        public CardExchangeType GetExchangeType(Card[] cards)
        {
            if (cards.Length != 3)
                return null;

            return ExchangeTypes.FirstOrDefault(exchangeType => exchangeType.CanExchange(cards));
        }
        
        
        
        
    }
}