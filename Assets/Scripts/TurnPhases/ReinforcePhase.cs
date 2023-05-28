using System;
using System.Linq;
using System.Text;
using Actions;
using Cards;
using Map;
using player;
using UnityEngine;

namespace TurnPhases
{
    public class ReinforcePhase : IPhase
    {
        public string Name => "Reinforce";

        private readonly GameManager _gm;
        private readonly CardRepository _cardRepository;

        public int RemainingTroopsToPlace => _remainingTroopsToPlace;
        private int _remainingTroopsToPlace;

        public Action OnTroopsToPlaceChanged;
        public Action<ReinforceAction> OnTroopsPlaced;
        public Action<ExchangeCardsAction> OnCardsExchanged;


        public ReinforcePhase(GameManager gm, CardRepository cardRepository)
        {
            _gm = gm;
            _cardRepository = cardRepository;

            OnCardsExchanged += LogCardExchange;
        }

        public void Start(Player player)
        {
            _remainingTroopsToPlace = player.GetTotalTroopBonus();
            OnTroopsToPlaceChanged?.Invoke();
            if (_remainingTroopsToPlace == 0 && !player.HasPossibleExchange()) _gm.NextTurnPhase();
        }

        public void OnAction(Player player, PlayerAction action)
        {
            if (action is ExchangeCardsAction exchangeCardsAction)
            {
                var cardExchange = exchangeCardsAction.Exchange;
                exchangeCardsAction.Player.RemoveCards(cardExchange.Cards);
                _cardRepository.ReturnCardsToDeck(cardExchange.Cards);
                _remainingTroopsToPlace += cardExchange.ExchangeValue;

                OnCardsExchanged?.Invoke(exchangeCardsAction);
                OnTroopsToPlaceChanged?.Invoke();
            }
            else if (action is ReinforceAction placeTroopsAction)
            {
                placeTroopsAction.Territory.AddTroops(placeTroopsAction.Troops);
                _remainingTroopsToPlace -= placeTroopsAction.Troops;

                OnTroopsPlaced?.Invoke(placeTroopsAction);
                OnTroopsToPlaceChanged?.Invoke();
            }
            else if (action is EndPhaseAction)
            {
                if (_remainingTroopsToPlace > 0)
                {
                    Debug.Log(
                        $"Player {player.Name} ended Reinforce phase with {_remainingTroopsToPlace} troops to place, distributing randomly");
                    player.RandomlyDistributeTroops(_remainingTroopsToPlace);
                    _remainingTroopsToPlace = 0;

                    OnTroopsToPlaceChanged?.Invoke();
                }

                _gm.NextTurnPhase();
                return;
            }
            else
                Debug.LogError($"ReinforcePhase: Received action of type {action.GetType().Name}");

            if (_remainingTroopsToPlace == 0)
                _gm.NextTurnPhase();
        }


        public void End(Player player)
        {
        }
        
        private void LogCardExchange(ExchangeCardsAction action)
        { 
            var msg = new StringBuilder($"Player {action.Player.Name} exchanged cards for {action.Exchange.ExchangeValue} troops.\nusing: ");

            var exchangeType = action.Exchange.ExchangeType;
            foreach (var (cardType, amount) in exchangeType.RequiredCards)
                msg.Append($"{amount} {cardType}, ");
            
            msg.AppendLine($" with {exchangeType.Troops} troops as base exchange value.");
            msg.Append("Cards exchanged: ");
            
            foreach (var card in action.Exchange.Cards)
                msg.Append($"{card.Name}: {card.Type}, ");

            msg.Remove(msg.Length - 2, 2);
            msg.Append(".");
            Debug.Log(msg.ToString());
        }
    }
}