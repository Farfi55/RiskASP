using System;
using System.Collections.Generic;
using System.Linq;
using player;
using UnityEngine;

namespace map
{
    class ContinentRepository : MonoBehaviour
    {
        public static ContinentRepository Instance { get; private set; }

        public Dictionary<string, Continent> Continents = new();
    
        [SerializeField] private bool loadFromChildren = true;
    
    
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
            if(loadFromChildren)
                LoadContinentsFromChildren();
        }

        private void LoadContinentsFromChildren()
        {
            if(Continents.Any())
                throw new Exception("Continents already loaded");

            foreach (var continent in GetComponentsInChildren<Continent>())
            {
                Add(continent);
            } 
        }

        public void Add(Continent continent)
        {
            if (Continents == null)
                Continents = new Dictionary<string, Continent>();
        
            Continents.Add(continent.Name, continent);
        }


        public int GetContinentBonusForPlayer(Player player)
        {
            var continentsBonus = 0;
            foreach (var continent in Continents.Values)
            {
                if (continent.IsOwnedByPlayer(player))
                    continentsBonus += continent.BonusTroops;
            }

            return continentsBonus;
        }
    
    }
}