using System;
using UnityEngine;

public class BattleSimulator : MonoBehaviour
{
    public static BattleSimulator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one BattleSimulator in the scene");
            Destroy(gameObject);
        }
        else
            Instance = this;
    }
}