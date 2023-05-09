using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{   
    [Id("attack_turn")]
    public class AttackTurnPredicate
    {
        [Param(0)] public int Turn;
        
        [Param(1)] public int AttackTurn;

        [Param(2)] public string Player;


        public AttackTurnPredicate()
        {
            
        }
        
        public AttackTurnPredicate(int turn,int attackTurn, string player)
        {
            Turn = turn;
            AttackTurn = attackTurn;
            Player = player;
        }
        
        public int setTurn(int turn) => Turn = turn;
        public int setAttackTurn(int attackTurn) => AttackTurn = attackTurn;
        public string setPlayer(string player) => Player = player;
        
        public int getTurn() => Turn;
        public int getAttackTurn() => AttackTurn;
        public string getPlayer() => Player;
    }
}