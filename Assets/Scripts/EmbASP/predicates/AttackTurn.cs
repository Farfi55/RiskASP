using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{   
    [Id("attack_turn")]
    public class AttackTurn
    {
        [Param(0)] private int _turn;
        
        [Param(1)] private int _attackTurn;

        [Param(2)] private string _player;
        
        

        public AttackTurn(int turn,int attackTurn, string player)
        {
            _turn = turn;
            _attackTurn = attackTurn;
            _player = player;
        }
    }
}