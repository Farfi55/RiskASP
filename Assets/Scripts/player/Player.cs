using System.Collections.Generic;
using System.Linq;
using map;
using UnityEngine;

namespace player
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private string _name;
        public string Name => _name;
        
        public HashSet<Territory> Territories;
        public PlayerColor Color;

        public void LoseTerritory(Territory territory)
        {
            Territories.Remove(territory);
        }

        public void WinTerritory(Territory territory)
        {
            Territories.Add(territory);
        }
    
        public int GetTroopsInTerritories()
        {
            return Territories.Sum(territory => territory.Troops);
        }
    
        public int GetTerritoryCount()
        {
            return Territories.Count;
        }
    
        public bool HasTerritory(Territory territory)
        {
            return Territories.Contains(territory);
        }
    
        public int GetTerritoryCountBonus()
        {
            return GetTerritoryCount() / 3;
        }
        
        public void SetName(string newName)
        {
            _name = newName;
            gameObject.name = $"Player {newName}";
        }
    
    
    
    
    

    }
}