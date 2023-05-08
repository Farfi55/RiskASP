using it.unical.mat.embasp.languages;
using UnityEngine.UI;

namespace EmbASP.predicates
{
    [Id("stop_attacking")]
    public class StopAttacking
    {
        [Param(0)] private int _turn;

        [Param(1)] private int _attackingTurn;
    }
}