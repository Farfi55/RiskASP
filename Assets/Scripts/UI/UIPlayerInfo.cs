using player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPlayerInfo : MonoBehaviour
    {
        [SerializeField] private Player _player;

        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _troopsText;
        [SerializeField] private TMP_Text _territoriesText;
        [SerializeField] private TMP_Text _cardsText;
        [SerializeField] private Image _colorImage;

        [SerializeField] private RectTransform _backgroundRectTransform;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private float _backgroundOffsetOnCurrentPlayer = 20f;
        private float _backgroundStartOffset;
        
        private Color _defaultBackgroundColor;
        [SerializeField] private Color _currentPlayerBackgroundColor;
        
        public Player Player
        {
            get => _player;
            set => SetPlayer(value);
        }


        private void Awake()
        {
            if (_player != null)
                SetPlayer(_player);
        }

        private void SetPlayer(Player value)
        {
            _defaultBackgroundColor = _backgroundImage.color;
            _player = value;
            _nameText.text = _player.Name;

            UpdatePlayerData();
            _player.OnCardsChanged += UpdateCardsData;
            _player.OnEliminated += (_) => _colorImage.color = _player.Color.Disabled;

            _backgroundStartOffset = _backgroundRectTransform.anchoredPosition.x;
            
            var gameManager = GameManager.Instance;
           
            OnPlayerTurnChanged(gameManager.CurrentPlayer);
            
            gameManager.OnPlayerTurnChanged += (_, newPlayer) => OnPlayerTurnChanged(newPlayer);
            gameManager.AttackPhase.OnAttacked += (_) => UpdatePlayerData();
            gameManager.OnGamePhaseChanged += (_) => UpdatePlayerData();
            gameManager.ReinforcePhase.OnTroopsPlaced += (_) => UpdatePlayerData();
        }

        private void OnPlayerTurnChanged(Player newPlayer)
        {
            if (newPlayer == _player)
            {
                _colorImage.color = _player.Color.Selected;
                var pos = Vector2.right * (_backgroundOffsetOnCurrentPlayer + _backgroundStartOffset);
                _backgroundRectTransform.anchoredPosition = pos;
                _backgroundImage.color = _currentPlayerBackgroundColor;
            }
            else
            {
                _colorImage.color = _player.Color.Normal;
                var pos = Vector2.right * _backgroundStartOffset;
                _backgroundRectTransform.anchoredPosition = pos;
                _backgroundImage.color = _defaultBackgroundColor;
            }

            if (_player.IsDead())
                _colorImage.color = _player.Color.Disabled;
        }

        private void UpdatePlayerData()
        {
            _troopsText.text = _player.GetTroopsCountInTerritories().ToString();
            _territoriesText.text = _player.GetTerritoryCount().ToString();
            UpdateCardsData();
        }

        private void UpdateCardsData()
        {
            _cardsText.text = _player.Cards.Count.ToString();
        }
    }
}