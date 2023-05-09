using System.Net;
using it.unical.mat.embasp.languages;
using UnityEditor;

namespace EmbASP.predicates
{
    [Id("after_attack_move")]
    public class AfterAttackMove
    {
        [Param(0)] public int Turn;

        [Param(1)] public int AttackTurn;

        [Param(2)] public string From;

        [Param(3)] public string To;

        [Param(4)] public int Troops;
    }
}