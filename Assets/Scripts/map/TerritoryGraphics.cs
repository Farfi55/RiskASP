using System;
using player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace map
{
    public class TerritoryGraphics : MonoBehaviour
    {
        

        [SerializeField] private Territory _territory;
        [SerializeField] private SpriteRenderer _territorySpriteRenderer;
        [SerializeField] private TMP_Text _troopsCountText;
        private PlayerColor _playerColor;

        private bool _isHovered;
        private bool _isSelected;
        
        private void Awake()
        {
            _territory.onStateChanged += UpdateGraphics;
        }


        private void UpdateGraphics()
        {
            _playerColor = _territory.Owner.Color;
            UpdateColor();
            SetTroopsCount(_territory.Troops);
        }

        private void UpdateColor()
        {
            if (_isSelected)
                SetColor(_playerColor.Disabled);
            else if (_isHovered)
                SetColor(_playerColor.Highlight);
            else
                SetColor(_playerColor.Normal);
        }

        private void SetTroopsCount(int territoryTroops)
        {
            _troopsCountText.text = territoryTroops.ToString();
        }

        private void SetColor(Color color)
        {
            _territorySpriteRenderer.color = color;
        }

        private void OnMouseEnter()
        {
            _isHovered = true;
            UpdateColor();
        }
        
        private void OnMouseExit()
        {
            _isHovered = false;
            UpdateColor();
        }
        
        private void OnMouseDown()
        {
            ToggleSelect();
        }

        private void ToggleSelect()
        {
            _isSelected = !_isSelected;
            UpdateColor();
        }

        public void Select()
        {
            _isSelected = true;
            UpdateColor();
        }
        
        public void Unselect()
        {
            _isSelected = false;
            UpdateColor();
        }
        
            
        
        
        
        
    }
}