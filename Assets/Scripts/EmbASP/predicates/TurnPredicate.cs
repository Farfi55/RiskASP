using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("turn")]
    public class TurnPredicate
    {
        [Param(0)] public int Turn;

        [Param(1)] public string Player;

        public TurnPredicate()
        {
            
        }
        
        public TurnPredicate(int turn, string player)
        {
            Turn = turn;
            Player = player;
        }
        
        public int setTurn(int turn) => Turn = turn;
        public string setPlayer(string player) => Player = player;
        
        public int getTurn() => Turn;
        public string getPlayer() => Player;
        
    }
}