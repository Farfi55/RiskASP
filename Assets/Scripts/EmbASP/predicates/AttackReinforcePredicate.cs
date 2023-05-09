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
        
        public AttackReinforcePredicate(int turn, int attackTurn, string from, string to, int troops)
        {
            Turn = turn;
            AttackTurn = attackTurn;
            From = from;
            To = to;
            Troops = troops;
        }
    }
}