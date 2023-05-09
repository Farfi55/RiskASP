using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("draft")]
    public class DraftPredicate
    {
        [Param(0)] public int Turn;

        [Param(1)] public string Territory;

        [Param(2)] public int Troops;


        public DraftPredicate()
        {
            
        }
        
        public DraftPredicate(int turn, string territory, int troops)
        {
            Turn = turn;
            Territory = territory;
            Troops = troops;
        }
        
        public int setTurn(int turn) => Turn = turn;
        public string setTerritory(string territory) => Territory = territory;
        public int setTroops(int troops) => Troops = troops;
        
        public int getTurn() => Turn;
        public string getTerritory() => Territory;
        public int getTroops() => Troops;
    }
}