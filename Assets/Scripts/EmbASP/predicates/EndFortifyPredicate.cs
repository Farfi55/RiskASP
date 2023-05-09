using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("end_fortify")]
    public class EndFortifyPredicate
    {
        [Param(0)] public int Turn;


        public EndFortifyPredicate()
        {
        }

        public EndFortifyPredicate(int turn)
        {
            Turn = turn;
        }
        
        public int setTurn(int turn) => Turn = turn;
        
        public int getTurn() => Turn;
    }
}