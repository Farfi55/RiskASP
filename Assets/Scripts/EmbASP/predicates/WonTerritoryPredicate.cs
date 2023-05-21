using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("won_territory")]
    public class WonTerritoryPredicate
    {

        [Param(0)] public int Turn;
        
        [Param(1)] public int AttackTurn;
        
        [Param(2)] public string Player;
        
        [Param(3)] public string Territory;
        
        public WonTerritoryPredicate()
        {
        }
        
        public WonTerritoryPredicate(int turn, int attackTurn, string player, string territory)
        {
            Turn = turn;
            AttackTurn = attackTurn;
            Player = player;
            Territory = territory;
        }
        
        
        public int setTurn(int turn) => Turn = turn;
        public int setAttackTurn(int attackTurn) => AttackTurn = attackTurn;
        public string setPlayer(string player) => Player = player;
        public string setTerritory(string territory) => Territory = territory;
        
        public int getTurn() => Turn;
        public int getAttackTurn() => AttackTurn;
        public string getPlayer() => Player;
        public string getTerritory() => Territory;
        
        
    }
}