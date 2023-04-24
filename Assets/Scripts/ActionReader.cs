using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ActionReader : MonoBehaviour
{
    public static ActionReader Instance { get; private set; }

    private Queue<PlayerAction> _actions = new();

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

    public void AddAction(PlayerAction action)
    {
        _actions.Enqueue(action);
    }
    
    
    
    

}