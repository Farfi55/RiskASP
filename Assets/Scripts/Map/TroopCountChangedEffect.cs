using System;
using TMPro;
using UnityEngine;

namespace Map
{
    internal class TroopCountChangedEffect : MonoBehaviour
    {
        [SerializeField] private TMP_Text _troopsCountText;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Territory _territory;
        [SerializeField] private float _timeToDisappear = 2f;
        [SerializeField] private float _fadeOutTime = 1f;
        [SerializeField] private float _yVelocity = 1f; 
        private float _remainingTimeToDisappear;


        private void Start()
        {
            _remainingTimeToDisappear = _timeToDisappear;
        }

        private void Update()
        {
            _remainingTimeToDisappear -= Time.deltaTime;
            transform.Translate(0f, _yVelocity * Time.deltaTime, 0f);
            UpdateAlpha();
            if (_remainingTimeToDisappear <= 0f)
            {
                Destroy(gameObject);
            }
        }

        private void UpdateAlpha()
        {
            var a = Math.Clamp(_remainingTimeToDisappear, 0f, _fadeOutTime) / _fadeOutTime;
            var srColor = _spriteRenderer.color;
            var color = new Color(srColor.r, srColor.g, srColor.b, a);
            _spriteRenderer.color = color;
            _troopsCountText.alpha = a;
        }

        public void SetTroopText(int amount)
        {
            _troopsCountText.text = amount.ToString();
        }

        public void SetTerritory(Territory territory)
        {
            _territory = territory;
            UpdateColor();
        }

        private void UpdateColor()
        {
            _spriteRenderer.color = _territory.Owner.Color.Normal;
        }
    }
}