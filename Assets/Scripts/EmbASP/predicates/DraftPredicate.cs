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

        public DraftPredicate(int turn, string player, string territory, int troops)
        {
            Turn = turn;
            Player = player;
            Territory = territory;
            Troops = troops;
        }
    }
}