using it.unical.mat.embasp.languages;
using UnityEngine.UI;

namespace EmbASP.predicates
{
    [Id("stop_attacking")]
    public class StopAttacking
    {
        [Param(0)] public int Turn;

        [Param(1)] public int AttackingTurn;

    }
}