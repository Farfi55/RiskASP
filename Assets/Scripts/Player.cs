using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string Name;
    public HashSet<Territory> Territories;
    public Color Color;

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
    
    
    
    
    

}