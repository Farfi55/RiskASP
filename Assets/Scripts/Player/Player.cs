using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Map;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace player
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private string _name;
        public string Name => _name;
        
        private readonly HashSet<Territory> _territories = new();
        public IEnumerable<Territory> Territories => _territories;
        public PlayerColor Color;

        
        public bool IsHuman() => GetComponent<HumanPlayer>() != null;
        public bool IsBot() => GetComponent<BotPlayer>() != null;
        
        
        public int GetTroopsCountInTerritories()
        {
            return _territories.Sum(territory => territory.Troops);
        }
    
        public int GetTerritoryCount()
        {
            return _territories.Count;
        }
    
        public bool HasTerritory(Territory territory)
        {
            return _territories.Contains(territory);
        }
    
        private int GetTerritoryCountBonus()
        {
            return GetTerritoryCount() / 3;
        }
        
        public void SetName(string newName)
        {
            _name = newName;
            gameObject.name = $"Player {newName}";
        }


        public int DistributeNTroopsPerTerritory(int troopsPerTerritory, int troops)
        {
            var territories = _territories.ToList();
            territories.Shuffle();
            for (int i = 0; i < territories.Count && troops > 0; i++)
            {
                int troopsToAdd = Mathf.Min(troopsPerTerritory, troops);
                territories[i].AddTroops(troopsToAdd);
                troops -= troopsToAdd;
            }
            return troops;
        }
        
        public void RandomlyDistributeTroops(int troops)
        {
            while (troops > 0)
            {
                var territory = _territories.ElementAt(Random.Range(0, _territories.Count));
                territory.AddTroops(1);
                troops--;
            }
        }

        public void AddTerritory(Territory territory) => _territories.Add(territory);
        public void RemoveTerritory(Territory territory) => _territories.Remove(territory);

        public int GetTotalTroopBonus()
        {
            var continentBonus = ContinentRepository.Instance.GetContinentBonusForPlayer(this);
            var territoriesBonus = GetTerritoryCountBonus();
            return territoriesBonus + continentBonus;
        }

        public bool IsDead() => _territories.Count == 0;
        public bool IsAlive() => _territories.Count > 0;

        public void ClearTroops()
        {
            foreach (var territory in _territories) territory.SetTroops(0);
        }
    }
}