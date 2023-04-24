using System.Runtime.InteropServices;
using it.unical.mat.embasp.languages;
using Unity.Mathematics;

namespace EmbASP.predicates
{
    [Id("attack_result")]
    public class AttackResult
    {
        [Param(0)] private int _turn;

        [Param(1)] private int _attackTurn;

        [Param(2)] private string _from;

        [Param(3)] private string _to;

        [Param(4)] private int _remainingTroopsAttacker;

        [Param(5)] private int _remainingTroopsDefender;
        
        //TODO: possible other params need
    }
}