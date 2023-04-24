using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using map;
using player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int _nPlayers = 2;
    public List<Player> Players;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        for (var i = 0; i < _nPlayers; i++)
        {
            Players.Add(PlayerCreator.Instance.NewPlayer());
        }

        TerritoryRepository.Instance.RandomlyAssignTerritories(Players);
        DistributeTroops();
    }

    private void DistributeTroops()
    {
        int[] troopsPerNumberOfPlayer = { -1, -1, 50, 35, 30, 25, 20 };
        int troopsPerPlayer = troopsPerNumberOfPlayer[_nPlayers];
        
        foreach (var player in Players)
            player.RandomlyDistributeTroops(troopsPerPlayer);
    }
}