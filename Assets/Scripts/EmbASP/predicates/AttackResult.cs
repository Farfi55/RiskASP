using System.Globalization;
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
    
        public AttackResult(int turn, int attackTurn, string from, string to, int remainingTroopsAttacker, int remainingTroopsDefender)
        {
            _turn = turn;
            _attackTurn = attackTurn;
            _from = from;
            _to = to;
            _remainingTroopsAttacker = remainingTroopsAttacker;
            _remainingTroopsDefender = remainingTroopsDefender;
        }


        public AttackResult(global::AttackResult attackResult)
        {
            var action = attackResult.AttackAction;
            _turn = action.Turn;
            _attackTurn = action.AttackTurn;
            _from = attackResult.Origin.Name;
            _to = attackResult.Target.Name;
            _remainingTroopsAttacker = attackResult.RemainingAttackingTroops;
            _remainingTroopsDefender = attackResult.RemainingDefendingTroops;
        }
    }
}