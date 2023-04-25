using System;
using System.Collections.Generic;
using System.Linq;
using Actions;
using Map;
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


    public AttackResult SimulateAttack(AttackAction attackAction)
    {
        var attackingTroops = Math.Min(attackAction.Troops, 3);
        var defendingTroops = Math.Min(attackAction.Target.Troops, 3);

        var battleWidth = Math.Min(attackingTroops, defendingTroops);

        var attackerRolls = RollDices(attackingTroops);
        var defenderRolls = RollDices(defendingTroops);

        Array.Sort(attackerRolls);
        Array.Sort(defenderRolls);

        var attackerLosses = 0;
        var defenderLosses = 0;

        for (int i = 0; i < battleWidth; i++)
        {
            if (attackerRolls[i] > defenderRolls[i])
                defenderLosses++;
            else
                attackerLosses++;
        }

        var remainingAttackingTroops = attackAction.Origin.Troops - attackerLosses;
        var remainingDefendingTroops = attackAction.Target.Troops - defenderLosses;
        
        return new AttackResult(
            attackAction,
            attackerLosses,
            defenderLosses,
            remainingAttackingTroops,
            remainingDefendingTroops,
            defendingTroops
        );
    }

    private int Roll() => UnityEngine.Random.Range(1, 7);

    private int[] RollDices(int dices)
    {
        var rolls = new int[dices];

        for (int i = 0; i < dices; i++)
        {
            rolls[i] = Roll();
        }

        return rolls;
    }
}