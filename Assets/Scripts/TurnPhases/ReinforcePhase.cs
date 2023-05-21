using System;
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


        public ReinforcePhase(GameManager gm, CardRepository cardRepository)
        {
            _gm = gm;
            _cardRepository = cardRepository;
        }

        public void Start(Player player)
        {
            _remainingTroopsToPlace = player.GetTotalTroopBonus();
            OnTroopsToPlaceChanged?.Invoke();
            if (_remainingTroopsToPlace == 0) _gm.NextTurnPhase();
        }

        public void OnAction(Player player, PlayerAction action)
        {
            if (action is ExchangeCardsAction exchangeCardsAction)
            {
                var cardExchange = exchangeCardsAction.Exchange;
                exchangeCardsAction.Player.RemoveCards(cardExchange.Cards);
                _cardRepository.ReturnCardsToDeck(cardExchange.Cards);
                _remainingTroopsToPlace += cardExchange.ExchangeValue;
                
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
    }
}