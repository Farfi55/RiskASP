using System;
using Actions;
using player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Map
{
    public class TerritoryGraphics : MonoBehaviour
    {
        [SerializeField] private Territory _territory;
        [SerializeField] private TerritorySelection _territorySelection;
        
        [SerializeField] private SpriteRenderer _territorySpriteRenderer;
        [SerializeField] private SpriteRenderer _troopsCountBackgroundSpriteRenderer;
        [SerializeField] private TMP_Text _troopsCountText;
        [SerializeField] private TroopCountChangedEffect _troopsChangedEffectPrefab;
        [SerializeField] private Transform _troopsChangedEffectParent;
        [SerializeField] private TMP_Text _territoryNameText;
        
        private PlayerColor _playerColor;

        private bool _isHovered;
        private bool _isSelected;
        private bool _isDisabled;
        
        private void Awake()
        {
            _territory.OnStateChanged += UpdateGraphics;
            if(_territorySelection != null)
                _territorySelection.OnStateChanged += OnStateChangedTerritorySelection;
            else 
                Debug.LogWarning("TerritorySelection is null");

            _territoryNameText.text = _territory.Name;
            
            _territory.OnTroopsChanged += OnTroopsChanged;
        }

        private void Start()
        {
            _playerColor = _territory.Owner.Color;
        }


        private void OnTroopsChanged(int oldTroops, int newTroops)
        {
            if(GameManager.Instance.GamePhase == GamePhase.Setup)
                return;
            
            var difference = newTroops - oldTroops;
            var troopCountChangedEffect = InstantiateTroopCountChangedEffect();
            troopCountChangedEffect.SetTroopText(difference);
        }


        private void OnStateChangedTerritorySelection(TerritorySelection territorySelection)
        {
            _isSelected = territorySelection.IsSelected;
            _isHovered = territorySelection.IsHovered;
            _isDisabled = territorySelection.IsDisabled;
            UpdateColor();
        }
        
        private TroopCountChangedEffect InstantiateTroopCountChangedEffect()
        {
            var troopCountChangedEffect = Instantiate(_troopsChangedEffectPrefab, _troopsChangedEffectParent);
            troopCountChangedEffect.SetTerritory(_territory);
            return troopCountChangedEffect;
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
                SetColor(_playerColor.Selected);
            else if(_isDisabled)
                SetColor(_playerColor.Disabled);
            else if (_isHovered)
                SetColor(_playerColor.Highlight);
            else
                SetColor(_playerColor.Normal);
            _troopsCountBackgroundSpriteRenderer.color = _playerColor.Normal;
        }

        private void SetTroopsCount(int territoryTroops)
        {
            _troopsCountText.text = territoryTroops.ToString();
        }

        private void SetColor(Color color)
        {
            _territorySpriteRenderer.color = color;
        }
    }
}