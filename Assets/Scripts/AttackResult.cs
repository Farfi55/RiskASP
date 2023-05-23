using System;
using Actions;
using Map;

public class AttackResult
{
    public AttackAction AttackAction { get; }

    public int AttackerLosses { get; }
    public int DefenderLosses { get; }

    /// <summary>
    /// all troops left in the attacking territory
    /// </summary>
    public int RemainingAttackingTroops { get; }

    public int RemainingAvailableAttackingTroops => RemainingAttackingTroops - 1;
    public int RemainingDefendingTroops { get; }

    public Territory Origin => AttackAction.Origin;
    public Territory Target => AttackAction.Target;
    public int AttackingTroops => AttackAction.Troops;
    public int DefendingTroops { get; }
    
    public int[] AttackerRolls { get; }
    public int[] DefenderRolls { get; }


public AttackResult(
        AttackAction attackAction,
        int attackerLosses,
        int defenderLosses,
        int remainingAttackingTroops,
        int remainingDefendingTroops,
        int defendingTroops,
        int[] attackerRolls,
        int[] defenderRolls
    )
    {
        AttackAction = attackAction;
        AttackerLosses = attackerLosses;
        DefenderLosses = defenderLosses;
        RemainingAttackingTroops = remainingAttackingTroops;
        RemainingDefendingTroops = remainingDefendingTroops;
        DefendingTroops = defendingTroops;
        AttackerRolls = attackerRolls;
        DefenderRolls = defenderRolls;
    }

    public bool HasAttackerWonTerritory() => RemainingDefendingTroops == 0;

    public int GetMinTroopsToMoveAfterWin() => Math.Min(Math.Min(AttackingTroops, 3), RemainingAvailableAttackingTroops);
}