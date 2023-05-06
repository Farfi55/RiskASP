using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using player;
using UnityEngine;

namespace Map
{
    public class TerritoryRepository : MonoBehaviour
    {
        public static TerritoryRepository Instance { get; private set; }
    
        public readonly Dictionary<string, Territory> Territories = new();
        private readonly Dictionary<Territory, int> _territoryToIslandMap = new(); 
    
        
        [SerializeField] private bool _loadFromChildren = true;
    

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
        
            if(_loadFromChildren)
                LoadTerritoriesFromChildren();
            
            SubscribeToCallbacks();
            CalculateTerritoryReachability();
        }

        private void SubscribeToCallbacks()
        {
            foreach (var territory in Territories.Values)
            {
                territory.OnOwnerChanged += (oldOwner, newOwner) =>
                {
                    OnTerritoryOwnerChanged(territory, oldOwner, newOwner);
                };
            }
        }

        private void OnTerritoryOwnerChanged(Territory territory, Player oldOwner, Player newOwner)
        {
            CalculateTerritoryReachability();            
        }

        private void CalculateTerritoryReachability()
        {
            // find territories islands (connected territories)
            // using BFS
            
            _territoryToIslandMap.Clear();
            var islandIndex = 0;
            foreach (var territory in Territories.Values)
            {
                if (_territoryToIslandMap.ContainsKey(territory))
                    continue;
                
                TerritoryReachabilityBFS(territory, islandIndex);
                islandIndex++;
            }

        }
        
        private void TerritoryReachabilityBFS(Territory territory, int islandIndex)
        {
            var queue = new Queue<Territory>();
            queue.Enqueue(territory);
            while (queue.Any())
            {
                var currentTerritory = queue.Dequeue();
                if (_territoryToIslandMap.ContainsKey(currentTerritory))
                    continue;

                _territoryToIslandMap.Add(currentTerritory, islandIndex);
                foreach (var neighbor in currentTerritory.NeighbourTerritories)
                {
                    if (neighbor.Owner != currentTerritory.Owner)
                        continue;

                    queue.Enqueue(neighbor);
                }
            }
        }

        private void LoadTerritoriesFromChildren()
        {
            if(Territories.Any())
                throw new Exception("Territories already loaded");

            foreach (var territory in GetComponentsInChildren<Territory>())
            {
                Add(territory);
            }
        }

        public void Add(Territory territory)
        {
            Territories.Add(territory.Name, territory);
        }

        public void RandomlyAssignTerritories(List<Player> players)
        {
            
            var territories = Territories.Values.ToList();
            territories.Shuffle();
            
            var playerOrder = Enumerable.Range(0, players.Count).ToList();
            playerOrder.Shuffle();
            
            var playerIndex = 0;
            foreach (var territory in territories)
            {
                var player = players[playerOrder[playerIndex]];
                territory.SetOwner(player);
                player.AddTerritory(territory);
                
                playerIndex = (playerIndex + 1) % players.Count;
            }

        }

        public bool CanReachTerritory(Territory from, Territory to)
        {
            return _territoryToIslandMap[from] == _territoryToIslandMap[to];
        }
    }
}