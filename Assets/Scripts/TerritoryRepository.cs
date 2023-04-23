using System.Collections.Generic;
using UnityEngine;

public class TerritoryRepository : MonoBehaviour
{
    public static TerritoryRepository Instance { get; private set; }
    
    public List<Territory> Territories;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }
}