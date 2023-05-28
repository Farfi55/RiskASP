using System;
using Cards;
using JetBrains.Annotations;
using player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICard : MonoBehaviour
    {
        [SerializeField] private Card _card;
        public Card Card => _card;

        [SerializeField] private Image _backgroundImage;
        [SerializeField] private GameObject _wildCardGfx;
        [SerializeField] private GameObject _defaultCardGfx;
        [SerializeField] private Image _cardTypeImage;
        [SerializeField] private Image _territoryImage;
        [SerializeField] private TMP_Text _territoryName;
        [SerializeField] private GameObject _extraTroopsMarker;
        
        private Color _backgroundColor;
        [SerializeField] private Color _selectedBackgroundColor;

        public bool IsSelected { get; private set; }

        public Action<UICard> OnClicked;
        
        private Player _playerOwner;
        private bool _playerOwnsTerritory;

        private void Awake()
        {
            _backgroundColor = _backgroundImage.color;
        }


        public void SetupWild(Card card)
        {
            if(card.Type != CardType.Wild)
                throw new ArgumentException("Card is not a wild card");
            _card = card;
            _wildCardGfx.SetActive(true);
            _defaultCardGfx.SetActive(false);
            
        }
        public void Setup(Card card, Sprite territorySprite, Color territoryColor, Sprite cardTypeSprite)
        {
            if(card.Type == CardType.Wild)
                throw new ArgumentException("Card is a wild card");
            _wildCardGfx.SetActive(false);
            _defaultCardGfx.SetActive(true);
            
            _card = card;
            _territoryImage.sprite = territorySprite;
            _territoryImage.color = territoryColor;

            if (card.Territory == null)
                throw new ArgumentException("Card has no territory");
            
            _territoryName.text = card.Territory.Name.Replace('_', ' ');
            _cardTypeImage.sprite = cardTypeSprite;

            _playerOwner = null;
            UpdateExtraTroopsMarker();
            card.Territory.OnOwnerChanged += (_,_) => UpdateExtraTroopsMarker();
        }


        private void UpdateExtraTroopsMarker()
        {
            if (_card.Territory != null 
                && _playerOwner != null 
                && _playerOwner == _card.Territory.Owner)
                _extraTroopsMarker.SetActive(true);
            else
                _extraTroopsMarker.SetActive(false);
        }

        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
            _backgroundImage.color = isSelected ? _selectedBackgroundColor : _backgroundColor;
        }

        
        public void Click()
        {
            OnClicked?.Invoke(this);
        }
        
        public void SetPlayerOwner([CanBeNull] Player player)
        {
            _playerOwner = player;
            UpdateExtraTroopsMarker();
        }
    }
}