using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("units_to_place")]
    public class UnitsToPlacePredicate
    {
        [Param(0)] public int Turn;
        
        [Param(1)] public string Player;

        [Param(2)] public int Number;

        public UnitsToPlacePredicate()
        {
            
        }
        
        public UnitsToPlacePredicate(int turn, string player, int number)
        {
            Turn = turn;
            Player = player;
            Number = number;
        }
        
        public int setTurn(int turn) => Turn = turn;
        public string setPlayer(string player) => Player = player;
        public int setNumber(int number) => Number = number;
        
        public int getTurn() => Turn;
        public string getPlayer() => Player;
        public int getNumber() => Number;
    }
}