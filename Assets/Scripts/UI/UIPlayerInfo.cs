using player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPlayerInfo : MonoBehaviour
    {
        private RectTransform _rectTransform;
        [SerializeField] private Player _player;

        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _troopsText;
        [SerializeField] private TMP_Text _territoriesText;
        [SerializeField] private TMP_Text _cardsText;
        [SerializeField] private Image _colorImage;

        [SerializeField] private float _widthOnCurrentPlayer = 320f;
        private float _defaultWidth = 300f;

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
            _rectTransform = GetComponent<RectTransform>();
            _defaultWidth = _rectTransform.rect.width;
            _player = value;
            _nameText.text = _player.Name;

            UpdatePlayerData();
            _player.OnCardsChanged += UpdateCardsData;
            _player.OnEliminated += (_) => _colorImage.color = _player.Color.Disabled;

            var gameManager = GameManager.Instance;
            _colorImage.color = gameManager.IsCurrentPlayer(_player) ? _player.Color.Selected : _player.Color.Normal;
            gameManager.OnPlayerTurnChanged += OnPlayerTurnChanged;
            gameManager.AttackPhase.OnAttacked += (_) => UpdatePlayerData();
        }

        private void OnPlayerTurnChanged(Player oldPlayer, Player newPlayer)
        {
            if (newPlayer == _player)
            {
                _colorImage.color = _player.Color.Selected;
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _widthOnCurrentPlayer);
            }
            else
            {
                _colorImage.color = _player.Color.Normal;
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _defaultWidth);
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