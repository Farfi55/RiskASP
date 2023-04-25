using System;
using UnityEngine;
using Action = it.unical.mat.embasp.languages.pddl.Action;

namespace Map
{
    [RequireComponent(typeof(Collider2D))]
    public class TerritorySelection : MonoBehaviour
    {
        public Territory Territory => _territory;
        [SerializeField] private Territory _territory;

        public bool IsHovered => _isHovered;
        public bool IsSelected => _isSelected;
        public bool IsDisabled => _isDisabled;

        private bool _isHovered;
        private bool _isSelected;
        private bool _isDisabled;

        public Action<TerritorySelection> OnSelected;
        public Action<TerritorySelection> OnUnselected;

        public Action<TerritorySelection> OnSelectedChanged;
        public Action<TerritorySelection> OnHoverChanged;
        public Action<TerritorySelection> OnDisabledChanged;
        public Action<TerritorySelection> OnStateChanged;
        
        public Action<TerritorySelection> OnClicked;


        private void OnMouseEnter() => SetHovered(true);

        private void OnMouseExit() => SetHovered(false);
        
        private void OnMouseDown()
        {
            if (!_isDisabled)
                OnClicked?.Invoke(this);
        }


        private void SetHovered(bool value)
        {
            _isHovered = value;
            OnHoverChanged?.Invoke(this);
            OnStateChanged?.Invoke(this);
        }

        public void SetSelected(bool value)
        {
            _isSelected = value;
            if (_isSelected) OnSelected?.Invoke(this);
            else OnUnselected?.Invoke(this);
            OnSelectedChanged?.Invoke(this);
            OnStateChanged?.Invoke(this);
        }

        public void SetDisabled(bool value)
        {
            _isDisabled = value;
            OnDisabledChanged?.Invoke(this);
            OnStateChanged?.Invoke(this);
        }


        public void ToggleSelect() => SetSelected(!_isSelected);
        public void Select() => SetSelected(true);
        public void Unselect() => SetSelected(false);

        public void Disable() => SetDisabled(true);
        public void Enable() => SetDisabled(false);
    }
}