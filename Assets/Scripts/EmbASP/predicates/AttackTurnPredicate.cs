using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{   
    [Id("attack_turn")]
    public class AttackTurnPredicate
    {
        [Param(0)] public int Turn;
        
        [Param(1)] public int AttackTurn;

        [Param(2)] public string Player;
        

        public AttackTurnPredicate(int turn,int attackTurn, string player)
        {
            Turn = turn;
            AttackTurn = attackTurn;
            Player = player;
        }
    }
}