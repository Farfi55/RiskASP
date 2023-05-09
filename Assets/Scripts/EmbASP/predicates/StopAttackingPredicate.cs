using it.unical.mat.embasp.languages;
using UnityEngine.UI;

namespace EmbASP.predicates
{
    [Id("stop_attacking")]
    public class StopAttackingPredicate
    {
        [Param(0)] public int Turn;

        [Param(1)] public int AttackingTurn;

        public StopAttackingPredicate(int turn, int attackingTurn)
        {
            Turn = turn;
            AttackingTurn = attackingTurn;
        }
    }
}