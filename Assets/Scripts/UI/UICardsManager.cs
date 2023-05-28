using System;
using System.Collections.Generic;
using System.Linq;
using Actions;
using Cards;
using Map;
using player;
using TMPro;
using TurnPhases;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class UICardsManager : MonoBehaviour
    {
        private GameManager _gameManager;
        private CardRepository _cardRepository;
        
        [SerializeField] private UICard _cardPrefab;
        [SerializeField] private Transform _cardsParent;
        [SerializeField] private HorizontalLayoutGroup _cardsLayoutGroup;

        [Space(10)]
        [SerializeField] private Button _exchangeButton;
        [SerializeField] private Button _raiseCardsButton;
        [SerializeField] private TMP_Text _exchangeCardsBonusText;
        [SerializeField] private TMP_Text _exchangeCardsCountText;
        
        [Space(10)]
        [SerializeField] private Sprite _infantryCardSprite;
        [SerializeField] private Sprite _cavalryCardSprite;
        [SerializeField] private Sprite _artilleryCardSprite;
        
        private List<Card> _selectedCards = new();
        private List<UICard> _currentPlayerCards = new();
        private Player _currentPlayer;

        private Dictionary<Card, UICard> _cardToCardUIMap = new();


        [SerializeField] private Sprite[] _territorySprites;
        private readonly Dictionary<string, Sprite> _territoryNameSpriteMap = new();
        
        private bool _areCardsRaised = false;
        
        public Action OnSelectedCardsChanged;
        
        

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _cardRepository = CardRepository.Instance;
            _gameManager.OnPlayerTurnChanged += (oldPlayer, newPlayer) => OnPlayerChanged(newPlayer);
            _gameManager.OnTurnPhaseChanged += OnTurnPhaseChanged;
            
            foreach (var sprite in _territorySprites) 
                _territoryNameSpriteMap.Add(sprite.name, sprite);


            foreach (var player in _gameManager.Players)
            {
                player.OnCardsChanged += () => OnPlayerCardsChanged(player);
            }
            OnSelectedCardsChanged += UpdateExchangeButton;
            OnSelectedCardsChanged += UpdateExchangeCardsBonusText;
            OnSelectedCardsChanged += UpdateExchangeCardsCountText;
            
            _exchangeButton.onClick.AddListener(ExchangeSelectedCards);
            _raiseCardsButton.onClick.AddListener(ToggleRaiseCards);
            
            CreateAllCards();
            UpdateExchangeButton();
            UpdateExchangeCardsBonusText();
            UpdateExchangeCardsCountText();
        }

        private void OnTurnPhaseChanged(IPhase oldPhase, IPhase newPhase)
        {
            UnselectCards();

            if(_areCardsRaised)
                ToggleRaiseCards();
        }

        private void ToggleRaiseCards()
        {
            _areCardsRaised = !_areCardsRaised;
            _cardsLayoutGroup.childAlignment = _areCardsRaised ? TextAnchor.LowerLeft : TextAnchor.UpperLeft;
            _raiseCardsButton.GetComponentInChildren<TMP_Text>().text = _areCardsRaised ? "v" : "^";
        }

        private void OnPlayerChanged(Player newPlayer)
        {
            
            _currentPlayer = newPlayer;
            
            DisableAllCards();
            UnselectCards();
            _currentPlayerCards.Clear();
            foreach (var card in newPlayer.Cards)
            {
                var uiCard = _cardToCardUIMap[card];
                uiCard.gameObject.SetActive(true);
                uiCard.SetPlayerOwner(newPlayer);
                _currentPlayerCards.Add(uiCard);
            }
            OnSelectedCardsChanged?.Invoke();
        }

        private void OnPlayerCardsChanged(Player player)
        {
            if(player != _currentPlayer)
                return;

            foreach (var currentPlayerCard in _currentPlayerCards)
            {
                if(!player.Cards.Contains(currentPlayerCard.Card))
                    currentPlayerCard.SetPlayerOwner(null);
            }
            DisableAllCards();
            foreach (var card in player.Cards)
            {
                var uiCard = _cardToCardUIMap[card];
                uiCard.gameObject.SetActive(true);
                uiCard.SetPlayerOwner(player);
            }
                
        }

        private void DisableAllCards()
        {
            foreach (var card in _cardToCardUIMap.Values)
            {
                if(card.IsSelected) 
                    card.SetSelected(false);
                card.gameObject.SetActive(false);
            } 
        }


        private void CreateAllCards()
        {
            foreach (var card in _cardRepository.AllCards)
            {
                var uiCard = CreateCard(card);
                _cardToCardUIMap.Add(card, uiCard);
                
                uiCard.OnClicked += OnCardClicked;
            }
        }

        private void OnCardClicked(UICard uiCard)
        {
            if(_gameManager.CurrentPhase != _gameManager.ReinforcePhase)
                return;
            
            if(_selectedCards.Contains(uiCard.Card))
            {
                uiCard.SetSelected(false);
                _selectedCards.Remove(uiCard.Card);
                OnSelectedCardsChanged?.Invoke();
            }
            else
            {
                if(_selectedCards.Count >= 3)
                    return;
                
                uiCard.SetSelected(true);
                _selectedCards.Add(uiCard.Card);
                OnSelectedCardsChanged?.Invoke();
            }
        }

        private UICard CreateCard(Card card)
        {
            var uiCard = Instantiate(_cardPrefab, _cardsParent);
            uiCard.gameObject.name = "card " + card.Name;
            if(card.Type == CardType.Wild)
                uiCard.SetupWild(card);
            else
            {
                var territorySprite = GetTerritorySprite(card.Territory);
                var cardTypeSprite = GetCardTypeSprite(card.Type);
                var territoryColor = GetRandomPlayerColor();
                uiCard.Setup(card, territorySprite, territoryColor, cardTypeSprite);
            }

            return uiCard;
        }

        private Color GetRandomPlayerColor()
        {
            // get a random player color from _gameManager.Players
            var randomPlayer = _gameManager.Players[Random.Range(0,_gameManager.Players.Count)];
            return randomPlayer.Color.Normal;
        }
        

        private Sprite GetTerritorySprite(Territory cardTerritory)
        {
            return _territoryNameSpriteMap[cardTerritory.Name];
        }

        private Sprite GetCardTypeSprite(CardType cardType)
        {
            return cardType switch
            {
                CardType.Infantry => _infantryCardSprite,
                CardType.Cavalry => _cavalryCardSprite,
                CardType.Artillery => _artilleryCardSprite,
                CardType.Wild => throw new ArgumentException("Card type is wild"),
                _ => throw new ArgumentOutOfRangeException(nameof(cardType), cardType, null)
            };
        }

        private bool CanExchangeSelectedCards()
        {
            return _cardRepository.CanExchange(_selectedCards.ToArray());
        }
        
        public void ExchangeSelectedCards()
        {
            var selectedCards = _selectedCards.ToArray();
            
            var cardExchange = _cardRepository.GetExchangeFor(selectedCards, _currentPlayer);
            if(cardExchange == null)
                return;
            
            var exchangeAction = new ExchangeCardsAction(_currentPlayer, _gameManager.Turn, cardExchange);
            ActionReader.Instance.AddAction(exchangeAction);

            UnselectCards();
        }
        
        
        private void UpdateExchangeCardsBonusText()
        {
            if (_selectedCards.Count < 3)
            {
                _exchangeCardsBonusText.text = "not enough cards";
            }
            else if (_selectedCards.Count == 3)
            {
                var exchange = _cardRepository.GetExchangeFor(_selectedCards.ToArray(), _currentPlayer);
                if(exchange != null)
                    _exchangeCardsBonusText.text = $"bonus: {exchange.ExchangeValue}";
                else
                    _exchangeCardsBonusText.text = "invalid combination";
            }
        }

        private void UpdateExchangeCardsCountText()
        {
            _exchangeCardsCountText.text = $"{_selectedCards.Count}/3";
        }


        private void UpdateExchangeButton()
        {
            var canExchange = _cardRepository.CanExchange(_selectedCards.ToArray());
            _exchangeButton.interactable = canExchange;
        }

        private void UnselectCards()
        {
            foreach (var selectedCard in _selectedCards)
            {
                _cardToCardUIMap[selectedCard].SetSelected(false);
            }
            _selectedCards.Clear();
            OnSelectedCardsChanged?.Invoke();
        }
        
    }
}