using System;
using player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIPlayerCreationManager : MonoBehaviour
    {
        [SerializeField] private UIPlayerCreationSlot[] _slots;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private StartingPlayersConfiguration _startingPlayersConfiguration;

        [SerializeField] public PlayerConfiguration[] PlayerConfigurations;
        [SerializeField] public PlayerColor[] PlayerColors;
        
        public int PlayerCount => _startingPlayersConfiguration.PlayersConfiguration.Count;
        
        
        private void Awake()
        {
            _startingPlayersConfiguration.PlayersConfiguration.Clear();
            
            _startGameButton.interactable = PlayerCount >= 2;
            _startGameButton.onClick.AddListener(StartGame);

            foreach (var uiPlayerCreationSlot in _slots)
            {
                uiPlayerCreationSlot.OnPlayerAdded += OnPlayerAdded;
                uiPlayerCreationSlot.OnPlayerRemove += OnPlayerRemove;
            }
        }

        private void OnPlayerRemove(PlayerCreationConfiguration playerConfiguration)
        {
            _startingPlayersConfiguration.PlayersConfiguration.Remove(playerConfiguration);
            _startGameButton.interactable = PlayerCount >= 2;
        }

        private void OnPlayerAdded(PlayerCreationConfiguration playerConfiguration)
        {
            _startingPlayersConfiguration.PlayersConfiguration.Add(playerConfiguration);
            _startGameButton.interactable = PlayerCount >= 2;
        }


        private void StartGame()
        {
            // load Main scene
            SceneManager.LoadScene("Main");
        }
    }
}
