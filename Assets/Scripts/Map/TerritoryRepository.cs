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

        public readonly Dictionary<string, Territory> TerritoriesMap = new();
        public readonly List<Territory> Territories = new();
        public readonly Dictionary<Territory, int> TerritoryToIslandMap = new();


        [SerializeField] private bool _loadFromChildren = true;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("More than one TerritoryRepository in scene");
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            if (_loadFromChildren)
                LoadTerritoriesFromChildren();

            SubscribeToCallbacks();
        }

        private void SubscribeToCallbacks()
        {
            foreach (var territory in Territories)
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

            TerritoryToIslandMap.Clear();
            var islandIndex = 0;
            foreach (var territory in Territories)
            {
                if (TerritoryToIslandMap.ContainsKey(territory))
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
                if (TerritoryToIslandMap.ContainsKey(currentTerritory))
                    continue;

                TerritoryToIslandMap.Add(currentTerritory, islandIndex);
                currentTerritory.GetComponent<TerritoryGraphics>()._territoryNameText.text = islandIndex.ToString();
                foreach (var neighbor in currentTerritory.NeighbourTerritories)
                {
                    if (neighbor.Owner == currentTerritory.Owner)
                        queue.Enqueue(neighbor);
                }
            }
        }

        private void LoadTerritoriesFromChildren()
        {
            if (Territories.Any())
                throw new Exception("Territories already loaded");

            foreach (var territory in GetComponentsInChildren<Territory>())
            {
                Add(territory);
            }
        }

        public void Add(Territory territory)
        {
            Territories.Add(territory);
            TerritoriesMap.Add(territory.Name, territory);
        }

        public void RandomlyAssignTerritories(List<Player> players)
        {
            var territories = Territories.ToList();
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

        public Territory FromName(string territoryName)
        {
            return TerritoriesMap[territoryName];
        }

        public bool CanFortifyTerritory(Territory from, Territory to)
        {
            return from.NeighbourTerritories.Contains(to) && from.Owner == to.Owner;
        }
    }
}