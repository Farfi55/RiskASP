using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace player
{
    public class PlayerCreator : MonoBehaviour
    {
        public static PlayerCreator Instance { get; private set; }
        
        
        public List<PlayerColor> _allPlayerColors = new();
        
        [FormerlySerializedAs("_playerPrefab")] [SerializeField] private Player _botPlayerPrefab;
        [SerializeField] private Player _humanPlayerPrefab;
        [SerializeField] private Transform _playerParent;
        
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("There is more than one PlayerCreator in the scene");
                Destroy(gameObject);
            } else {
                Instance = this;
            }
        }
        
        
        public Player CreateHumanPlayer()
        {
            var player = Instantiate(_humanPlayerPrefab, _playerParent);
            SetUpPlayerFromRandomColor(player);
            return player;
        }
        
        public Player CreateBotPlayer()
        {
            var player = Instantiate(_botPlayerPrefab, _playerParent);
            SetUpPlayerFromRandomColor(player);
            return player;
        }
        
        public Player CreatePlayerFromConfiguration(PlayerCreationConfiguration playerCreationConfiguration)
        {
            var playerConfiguration = playerCreationConfiguration.PlayerConfiguration;
            Player player;
            if(playerConfiguration is BotConfiguration)
                player = CreateBotPlayer();
            else if(playerConfiguration is HumanPlayerConfiguration)
                player = CreateHumanPlayer();
            else
                throw new Exception("PlayerConfiguration not supported");
        
            player.Color = playerCreationConfiguration.PlayerColor;
            player.SetName(playerCreationConfiguration.PlayerName);
            return player;
        }

        
        public void SetUpPlayerFromRandomColor(Player player)
        {
            PlayerColor color = GetRandomUnusedColor();
            SetUpPlayerFromColor(player, color);
        }
        
        public void SetUpPlayerFromColor(Player player, PlayerColor color)
        {
            player.Color = color;
            player.SetName(color.name);
        }

        private PlayerColor GetRandomUnusedColor()
        {
            var players = GameManager.Instance.Players;

            var unusedColors = _allPlayerColors.Except(players.Select(p => p.Color)).ToList();
            if (unusedColors.Count == 0)
                throw new Exception("No more colors available");
            return unusedColors[UnityEngine.Random.Range(0, unusedColors.Count)];
        }
    }
}