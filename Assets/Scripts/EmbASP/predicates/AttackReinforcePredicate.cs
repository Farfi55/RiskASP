using System.Net;
using it.unical.mat.embasp.languages;
using UnityEditor;

namespace EmbASP.predicates
{
    [Id("attack_reinforce")]
    public class AttackReinforcePredicate
    {
        [Param(0)] public int Turn;

        [Param(1)] public int AttackTurn;

        [Param(2)] public string From;

        [Param(3)] public string To;

        [Param(4)] public int Troops;


        public AttackReinforcePredicate()
        {
        }

        public AttackReinforcePredicate(int turn, int attackTurn, string from, string to, int troops)
        {
            Turn = turn;
            AttackTurn = attackTurn;
            From = from;
            To = to;
            Troops = troops;
        }
        
        public int setTurn(int turn) => Turn = turn;
        public int setAttackTurn(int attackTurn) => AttackTurn = attackTurn;
        public string setFrom(string from) => From = from;
        public string setTo(string to) => To = to;
        public int setTroops(int troops) => Troops = troops;
        
        public int getTurn() => Turn;
        public int getAttackTurn() => AttackTurn;
        public string getFrom() => From;
        public string getTo() => To;
        public int getTroops() => Troops;
    }
}