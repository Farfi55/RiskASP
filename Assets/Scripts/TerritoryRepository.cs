using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerritoryRepository : MonoBehaviour
{
    public static TerritoryRepository Instance { get; private set; }
    
    public Dictionary<string, Territory> Territories = new();
    
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
            LoadTerritoriesFromChildren();
        
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
        if (Territories == null)
            Territories = new Dictionary<string, Territory>();
        
        Territories.Add(territory.Name, territory);
    }
}