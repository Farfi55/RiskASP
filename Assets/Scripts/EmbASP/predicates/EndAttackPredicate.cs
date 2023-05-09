using it.unical.mat.embasp.languages;
using UnityEngine.UI;

namespace EmbASP.predicates
{
    [Id("end_attack")]
    public class EndAttackPredicate
    {
        [Param(0)] public int Turn;

        [Param(1)] public int AttackingTurn;

        public EndAttackPredicate()
        {
            
        }
        
        public EndAttackPredicate(int turn, int attackingTurn)
        {
            Turn = turn;
            AttackingTurn = attackingTurn;
        }
        
        public int setTurn(int turn) => Turn = turn;
        public int setAttackingTurn(int attackingTurn) => AttackingTurn = attackingTurn;
        
        public int getTurn() => Turn;
        public int getAttackingTurn() => AttackingTurn;
        
    }
}