using System.Net;
using System.Runtime.InteropServices;
using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("fortify")]
    public class FortifyPredicate
    {
        [Param(0)] public int Turn;

        [Param(1)] public string From;

        [Param(2)] public string To;

        [Param(3)] public int Troops;


        public FortifyPredicate(int turn, string from, string to, int troops)
        {
            Turn = turn;
            From = from;
            To = to;
            Troops = troops;
        }
    }
}