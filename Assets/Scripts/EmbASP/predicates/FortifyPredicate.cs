using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.InteropServices;
using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("fortify")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FortifyPredicate
    {
        [Param(0)] public int Turn;

        [Param(1)] public string From;

        [Param(2)] public string To;

        [Param(3)] public int Troops;


        public FortifyPredicate()
        {
            
        }
        
        public FortifyPredicate(int turn, string from, string to, int troops)
        {
            Turn = turn;
            From = from;
            To = to;
            Troops = troops;
        }
        
        public int setTurn(int turn) => Turn = turn;
        public string setFrom(string from) => From = from;
        public string setTo(string to) => To = to;
        public int setTroops(int troops) => Troops = troops;
        
        public int getTurn() => Turn;
        public string getFrom() => From;
        public string getTo() => To;
        public int getTroops() => Troops;
        
    }
}