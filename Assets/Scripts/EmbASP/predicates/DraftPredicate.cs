using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("draft")]
    public class DraftPredicate
    {
        [Param(0)] public int Turn;

        [Param(1)] public string Player;

        [Param(2)] public string Territory;

        [Param(3)] public int Troops;


        public DraftPredicate()
        {
            
        }
        
        public DraftPredicate(int turn, string player, string territory, int troops)
        {
            Turn = turn;
            Player = player;
            Territory = territory;
            Troops = troops;
        }
        
        public int setTurn(int turn) => Turn = turn;
        public string setPlayer(string player) => Player = player;
        public string setTerritory(string territory) => Territory = territory;
        public int setTroops(int troops) => Troops = troops;
        
        public int getTurn() => Turn;
        public string getPlayer() => Player;
        public string getTerritory() => Territory;
        public int getTroops() => Troops;
    }
}