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
            SetUpPlayerFromColor(player);
            return player;
        }
        
        public Player CreateBotPlayer()
        {
            var player = Instantiate(_botPlayerPrefab, _playerParent);
            SetUpPlayerFromColor(player);
            return player;
        }

        
        public void SetUpPlayerFromColor(Player player)
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