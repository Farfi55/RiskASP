using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("troops_to_place")]
    public class TroopsToPlacePredicate
    {
        [Param(0)] public int Turn;
        
        [Param(1)] public string Player;

        [Param(2)] public int Troops;

        public TroopsToPlacePredicate()
        {
            
        }
        
        public TroopsToPlacePredicate(int turn, string player, int troops)
        {
            Turn = turn;
            Player = player;
            Troops = troops;
        }
        
        public int setTurn(int turn) => Turn = turn;
        public string setPlayer(string player) => Player = player;
        public int setTroops(int number) => Troops = number;
        
        public int getTurn() => Turn;
        public string getPlayer() => Player;
        public int getTroops() => Troops;
    }
}