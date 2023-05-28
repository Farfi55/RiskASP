using System;
using Cards;
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

        private Color _backgroundColor;
        [SerializeField] private Color _selectedBackgroundColor;

        public bool IsSelected { get; private set; }

        public Action<UICard> OnClicked; 


        private void Awake()
        {
            _backgroundColor = _backgroundImage.color;
        }


        public void SetupWild(Card card)
        {
            if(card.Type != CardType.Wild)
                throw new ArgumentException("Card is not a wild card");
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
            
            _territoryName.text = card.Territory!.Name.Replace('_', ' ');
            _cardTypeImage.sprite = cardTypeSprite;
        }

        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
            _backgroundImage.color = isSelected ? _selectedBackgroundColor : _backgroundColor;
        }

        private void OnMouseDown()
        {
            Debug.Log("UICard.OnMouseDown");
            OnClicked?.Invoke(this);
        }
    }
}