using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Continent : MonoBehaviour
{
    public string Name;
    public List<Territory> Territories = new();
    public int BonusTroops;
    public Color Color;


    private void Start()
    {
        SetupCheck();
    }

    private void SetupCheck()
    {
        if (Territories.Count == 0)
            Debug.LogError($"Continent {Name} has no territories", this);

        var territory = Territories.FirstOrDefault(t => t.Continent != this);
        if (territory != null)
            Debug.LogError($"Continent {Name} has territory {territory.Name} that does not belong to it", this);
        
        if(BonusTroops == 0)
            Debug.LogWarning($"Continent {Name} has no bonus troops", this);
    }
    
    
    public bool IsComplete()
    {
        var owner = Territories.First().Owner;
        return Territories.All(t => t.Owner == owner);
    }
    
    public Player GetOwner()
    {
        if (!IsComplete())
            return null;
        return Territories.First().Owner;
    }

    public bool IsOwnedByPlayer(Player player)
    {
        return Territories.All(t => t.Owner == player);
    }



    [MenuItem("CONTEXT/Continent/load child territories")]
    static void DoubleMass(MenuCommand command)
    {
        Continent continent = (Continent)command.context;
        continent.Territories = new ();
        foreach (Transform child in continent.transform)
        {
            continent.Territories.Add(child.GetComponent<Territory>());
        }
        // not needed as Continent is not a prefab
        // PrefabUtility.RecordPrefabInstancePropertyModifications(continent);
        
    }

}