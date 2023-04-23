using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Territory : MonoBehaviour
{
    public string Name;
    public Continent Continent;
    public List<Territory> NeighbourTerritories;
    public Player Owner;
    public int Troops;

    public Action onStateChanged;


    public void Start()
    {
        SetupChecks();
    }

    private void SetupChecks()
    {
        if (NeighbourTerritories.Count == 0)
            Debug.LogError("Territory " + Name + " has no neighbours");

        if (Continent == null)
            Debug.LogError("Territory " + Name + " has no continent");

        if (NeighbourTerritories.Contains(this))
            Debug.LogError("Territory " + Name + " has itself as neighbour");
    }


    public void AddTroops(int amount)
    {
        Troops += amount;
        onStateChanged?.Invoke();
    }

    public void RemoveTroops(int amount)
    {
        Troops -= amount;
        if (Troops < 0)
        {
            Debug.LogError($"Troops of {Name} went negative ({Troops})!");
            Troops = 0;
        }

        onStateChanged?.Invoke();
    }

    public void SetOwner(Player newOwner)
    {
        Player oldOwner = Owner;
        if (oldOwner == newOwner)
        {
            Debug.LogWarning($"Tried to set owner of {Name} to {newOwner.Name}, but it already is owned by {newOwner.Name}!");
            return;
        }

        oldOwner.LoseTerritory(this);
        newOwner.WinTerritory(this);
        Owner = newOwner;
        onStateChanged?.Invoke();
    }


    public List<Territory> GetEnemyNeighbours()
    {
        return NeighbourTerritories.FindAll(neighbour => neighbour.Owner != Owner);
    }

    public List<Territory> GetFriendlyNeighbours()
    {
        return NeighbourTerritories.FindAll(neighbour => neighbour.Owner == Owner);
    }


    public bool IsNeighbourOf(Territory other)
    {
        return NeighbourTerritories.Contains(other);
    }

    public bool IsNeighbourOf(Player player)
    {
        return NeighbourTerritories.Any(neighbour => neighbour.Owner == player);
    }

    public bool HasAnyNeighbourEnemy()
    {
        return NeighbourTerritories.Any(neighbour => neighbour.Owner != Owner);
    }

    public bool HasAnyNeighbourFriendly()
    {
        return NeighbourTerritories.Any(neighbour => neighbour.Owner == Owner);
    }
    
    public int GetAvailableTroopsToAttack()
    {
        return Troops - 1;
    }


    public override string ToString()
    {
        return $"Territory {Name},  ({Troops} troops) owned by {Owner.Name}";
    }


    [MenuItem("CONTEXT/Territory/load data")]
    static void DoubleMass(MenuCommand command)
    {
        Territory territory = (Territory)command.context;
        territory.Name = territory.gameObject.name;
        territory.Continent = territory.transform.parent.GetComponent<Continent>();
    }
}