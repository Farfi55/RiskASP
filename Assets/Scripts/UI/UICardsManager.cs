using System;
using System.Collections.Generic;
using Cards;
using player;
using UnityEngine;

namespace UI
{
    public class UICardsManager : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField] private UICard _cardPrefab;
        [SerializeField] private Transform _cardsParent;
        
        private List<UICard> _selectedCards = new();
        private List<UICard> _currentPlayerCards = new();

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _gameManager.OnPlayerTurnChanged += (oldPlayer, newPlayer) => OnPlayerChanged(newPlayer);
        }

        private void CreateCard(Card card)
        {
            var uiCard = Instantiate(_cardPrefab, _cardsParent);
            uiCard.SetCard(card);
            _currentPlayerCards.Add(uiCard);
        }

        private void OnPlayerChanged(Player newPlayer)
        {
            
        }
    }
}