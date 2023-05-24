using System;
using Cards;
using UnityEngine;

namespace UI
{
    public class UICard : MonoBehaviour
    {
        [SerializeField] private Card _card;
        
        // [SerializeField] private Sprite _
        
        public Card Card
        {
            get => _card;
            set => SetCard(value);
        }

        public void SetCard(Card card)
        {
            _card = card;
            
        }

        private void OnMouseDown()
        {
                
        }
        

    }
}