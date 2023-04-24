using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ActionReader : MonoBehaviour
{
    public static ActionReader Instance { get; private set; }

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
    
    Queue<ReinforceAction> _reinforceActions = new();


    public IEnumerator<ReinforceAction> ReadNextReinforceAction()
    {
        while (_reinforceActions.Count == 0)
        {
            // TODO: implement better solution
            yield return null;
        }
        yield return _reinforceActions.Dequeue();
    }
}