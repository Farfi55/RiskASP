using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace player
{
    public class PlayerCreator : MonoBehaviour
    {
        public static PlayerCreator Instance { get; private set; }
        
        
        public List<PlayerColor> _allPlayerColors = new();
        
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Transform _playerParent;
        
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
        }
        
        public Player NewPlayer()
        {
            PlayerColor color = GetRandomUnusedColor();
            var player = Instantiate(_playerPrefab, _playerParent);
            player.Color = color;
            player.SetName(color.name);
            return player;
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