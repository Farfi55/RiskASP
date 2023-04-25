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
    public int RemainingDefendingTroops { get; }

    public Territory Origin => AttackAction.Origin;
    public Territory Target => AttackAction.Target;
    public int AttackingTroops => AttackAction.Troops;
    public int DefendingTroops { get; }
    

    public AttackResult(
        AttackAction attackAction, 
        int attackerLosses, 
        int defenderLosses, 
        int remainingAttackingTroops, 
        int remainingDefendingTroops, 
        int defendingTroops)
    {
        AttackAction = attackAction;
        AttackerLosses = attackerLosses;
        DefenderLosses = defenderLosses;
        RemainingAttackingTroops = remainingAttackingTroops;
        RemainingDefendingTroops = remainingDefendingTroops;
        DefendingTroops = defendingTroops;
    }
    
    public bool HasAttackerWonTerritory() => RemainingDefendingTroops == 0;

    public int MaxPossibleLosses() => Math.Min(AttackingTroops, DefendingTroops);

}