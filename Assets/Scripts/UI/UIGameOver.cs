using System;
using player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIGameOver : MonoBehaviour
    {
        private GameManager _gameManager;
        
        [SerializeField] private Image _backgroundTintImage;
        [SerializeField] private TMP_Text _gameOverInfoText;
        [SerializeField] private Button _mainMenuButton;

        private void Awake()
        {
            
            _gameManager = GameManager.Instance;
            _gameManager.OnGamePhaseChanged += OnGamePhaseChanged;
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            gameObject.SetActive(false);
        }

        private void OnGamePhaseChanged(GamePhase phase)
        {
            if (phase == GamePhase.Over) 
                GameOver();
        }

        private void GameOver()
        {
            Player winner = _gameManager.Players.Find(player => player.IsAlive());
            int turn = _gameManager.Turn;
            int troops = winner.GetTroopsCountInTerritories();

            string message = $"CONGRATULATIONS!\nPlayer {winner.Name} won in {turn} turns with {troops} troops left!";
            message += "\ndefeated players:\n";
            foreach (var player in _gameManager.Players)
            {
                if (player.IsDead())
                    message += $"{player.Name}\n";
            }

            _gameOverInfoText.text = message;
            _backgroundTintImage.gameObject.SetActive(true);
            
            gameObject.SetActive(true);
        }
        
        private void OnMainMenuButtonClicked()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
