using System;
using player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class UIPlayerCreationSlot : MonoBehaviour
    {

        [SerializeField] private UIPlayerCreationManager _uiPlayerCreationManager;
        public PlayerCreationConfiguration PlayerCreationConfiguration { get; private set; }

        public bool PlayerCreated => PlayerCreationConfiguration != null;
        
        
        [SerializeField] private Button _addPlayerButton;
        [SerializeField] private Button _removePlayerButton;
        [SerializeField] private TMP_InputField _playerNameInputField;
        [SerializeField] private TMP_Dropdown _playerColorDropdown;
        [SerializeField] private TMP_Dropdown _playerConfigurationDropdown;

        public Action<PlayerCreationConfiguration> OnPlayerAdded;
        public Action<PlayerCreationConfiguration> OnPlayerRemove;
        
        public Action<PlayerCreationConfiguration> OnPlayerCreationConfigurationChanged;
        [SerializeField] private GameObject _playerCreatedGfx;
        [SerializeField] private GameObject _playerNotCreatedGfx;

        private void Awake()
        {
            if(_uiPlayerCreationManager == null)
                _uiPlayerCreationManager = FindObjectOfType<UIPlayerCreationManager>();
            
            PlayerCreationConfiguration = null;
            
            _playerCreatedGfx.SetActive(PlayerCreated);
            _playerNotCreatedGfx.SetActive(!PlayerCreated);
            
            GenerateDropdownItems();
            
            
            _addPlayerButton.onClick.AddListener(AddPlayer);
            _removePlayerButton.onClick.AddListener(RemovePlayer);
            _playerNameInputField.onValueChanged.AddListener(OnPlayerNameChanged);
            _playerColorDropdown.onValueChanged.AddListener(OnPlayerColorChanged);
            _playerConfigurationDropdown.onValueChanged.AddListener(OnPlayerConfigurationChanged);
        }

        private void GenerateDropdownItems()
        {
            var playerConfigurations = _uiPlayerCreationManager.PlayerConfigurations;
            var playerColors = _uiPlayerCreationManager.PlayerColors;
            
            _playerConfigurationDropdown.ClearOptions();
            _playerColorDropdown.ClearOptions();
            
            foreach (var playerConfiguration in playerConfigurations)
                _playerConfigurationDropdown.options.Add(new TMP_Dropdown.OptionData(playerConfiguration.Name));

            foreach (var playerColor in playerColors)
                _playerColorDropdown.options.Add(new TMP_Dropdown.OptionData(playerColor.name));
            
            _playerConfigurationDropdown.RefreshShownValue();
            _playerColorDropdown.RefreshShownValue();
        }

        private void OnPlayerConfigurationChanged(int index)
        {
            var playerConfiguration = _uiPlayerCreationManager.PlayerConfigurations[index];
            PlayerCreationConfiguration.PlayerConfiguration = playerConfiguration;
            OnPlayerCreationConfigurationChanged?.Invoke(PlayerCreationConfiguration);
        }

        private void OnPlayerColorChanged(int index)
        {
            var playerColor = _uiPlayerCreationManager.PlayerColors[index];
            PlayerCreationConfiguration.PlayerColor = playerColor;
            OnPlayerCreationConfigurationChanged?.Invoke(PlayerCreationConfiguration);
        }

        private void OnPlayerNameChanged(string name)
        {
            if(!PlayerCreated) return;

            PlayerCreationConfiguration.PlayerName = name;
            OnPlayerCreationConfigurationChanged?.Invoke(PlayerCreationConfiguration);
        }


        private void AddPlayer()
        {
            Debug.Log("AddPlayer");
            if(PlayerCreated) return;

            var playerConfiguration = _uiPlayerCreationManager.PlayerConfigurations[0];
            var colorIndex = Random.Range(0, _uiPlayerCreationManager.PlayerColors.Length);
            var playerColor = _uiPlayerCreationManager.PlayerColors[colorIndex];
            PlayerCreationConfiguration = new PlayerCreationConfiguration
            {
                PlayerColor = playerColor,
                PlayerConfiguration = playerConfiguration, 
                PlayerName = "Player " + (_uiPlayerCreationManager.PlayerCount + 1),
            };
            
            _playerNameInputField.text = PlayerCreationConfiguration.PlayerName;
            _playerColorDropdown.value = colorIndex;
            _playerConfigurationDropdown.value = 0;

            _playerCreatedGfx.SetActive(PlayerCreated);
            _playerNotCreatedGfx.SetActive(!PlayerCreated);
            
            OnPlayerAdded?.Invoke(PlayerCreationConfiguration);
            
        }

        private void RemovePlayer()
        {
            Debug.Log("RemovePlayer");
            if(!PlayerCreated) return;
            
            OnPlayerRemove?.Invoke(PlayerCreationConfiguration);
            
            PlayerCreationConfiguration = null;
            _playerCreatedGfx.SetActive(PlayerCreated);
            _playerNotCreatedGfx.SetActive(!PlayerCreated);
            
        }

    }
}
