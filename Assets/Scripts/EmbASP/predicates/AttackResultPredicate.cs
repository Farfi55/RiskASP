using System.Globalization;
using System.Runtime.InteropServices;
using it.unical.mat.embasp.languages;
using Unity.Mathematics;

namespace EmbASP.predicates
{
    [Id("attack_result")]
    public class AttackResultPredicate
    {
        [Param(0)] public int Turn;

        [Param(1)] public int AttackTurn;

        [Param(2)] public string From;

        [Param(3)] public string To;

        [Param(4)] public int RemainingTroopsAttacker;

        [Param(5)] public int RemainingTroopsDefender;

        public AttackResultPredicate()
        {
            
        }
    
        public AttackResultPredicate(int turn, int attackTurn, string from, string to, int remainingTroopsAttacker, int remainingTroopsDefender)
        {
            Turn = turn;
            AttackTurn = attackTurn;
            From = from;
            To = to;
            RemainingTroopsAttacker = remainingTroopsAttacker;
            RemainingTroopsDefender = remainingTroopsDefender;
        }


        public AttackResultPredicate(global::AttackResult attackResult)
        {
            var action = attackResult.AttackAction;
            Turn = action.Turn;
            AttackTurn = action.AttackTurn;
            From = attackResult.Origin.Name;
            To = attackResult.Target.Name;
            RemainingTroopsAttacker = attackResult.RemainingAttackingTroops;
            RemainingTroopsDefender = attackResult.RemainingDefendingTroops;
        }
        
        public int setTurn(int turn) => Turn = turn;
        public int setAttackTurn(int attackTurn) => AttackTurn = attackTurn;
        public string setFrom(string from) => From = from;
        public string setTo(string to) => To = to;
        public int setRemainingTroopsAttacker(int troops) => RemainingTroopsAttacker = troops;
        public int setRemainingTroopsDefender(int troops) => RemainingTroopsDefender = troops;
        
        
        public int getTurn() => Turn;
        public int getAttackTurn() => AttackTurn;
        public string getFrom() => From;
        public string getTo() => To;
        public int getRemainingTroopsAttacker() => RemainingTroopsAttacker;
        public int getRemainingTroopsDefender() => RemainingTroopsDefender;
        
    }
}