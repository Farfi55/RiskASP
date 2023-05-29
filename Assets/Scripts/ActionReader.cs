using System;
using System.Collections;
using System.Collections.Generic;
using Actions;
using UnityEngine;

public class ActionReader : MonoBehaviour
{
    public static ActionReader Instance { get; private set; }

    [SerializeField] public bool Paused = false;
    [SerializeField][Range(0f,5f)] public float BotActionDelay = 0.5f;
    private float _remainingActionDelay = 0f;
    
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

    private void Update()
    {
        if(Paused) 
            return;
        
        _remainingActionDelay -= Time.deltaTime;
        
        while (_actions.Count > 0 && _remainingActionDelay < 0f)
        {
            var action = _actions.Dequeue();
            GameManager.Instance.HandlePlayerAction(action);
            if(action.Player.IsBot())
                _remainingActionDelay = BotActionDelay;
        }
    }
}