using System;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    [CreateAssetMenu(fileName = "StartingPlayersConfiguration", menuName = "StartingPlayersConfiguration", order = 1)]
    public class StartingPlayersConfiguration : ScriptableObject
    {
        [SerializeField]
        public List<PlayerCreationConfiguration> PlayersConfiguration = new();
    }

    [Serializable]
    public class PlayerCreationConfiguration
    {
        [SerializeField] private PlayerConfiguration _playerConfiguration;
        [SerializeField] private string _playerName;
        [SerializeField] private PlayerColor _playerColor;

        public PlayerConfiguration PlayerConfiguration
        {
            get => _playerConfiguration;
            set => _playerConfiguration = value;
        }

        public string PlayerName
        {
            get => _playerName;
            set => _playerName = value;
        }
        public PlayerColor PlayerColor
        {
            get => _playerColor;
            set => _playerColor = value;
        }

        public PlayerCreationConfiguration()
        {
            PlayerConfiguration = null;
            PlayerName = "";
            PlayerColor = null;
        }
    }
}