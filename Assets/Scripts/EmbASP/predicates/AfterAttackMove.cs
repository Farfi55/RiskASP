using System.Net;
using it.unical.mat.embasp.languages;
using UnityEditor;

namespace EmbASP.predicates
{
    [Id("after_attack_move")]
    public class AfterAttackMove
    {
        [Param(0)] private int _turn;

        [Param(1)] private int _attackingTurn;

        [Param(2)] private string _from;

        [Param(3)] private string _to;

        [Param(4)] private int _armies;
    }
}