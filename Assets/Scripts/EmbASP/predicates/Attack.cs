using System.Runtime.InteropServices;
using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("attack")]
    public class Attack
    {
        [Param(0)] private int _turn;

        [Param(1)] private int _attackTurn;

        [Param(2)] private string _from;

        [Param(3)] private string _to;

        [Param(4)] private int _armies;

        public string From
        {
            get => _from;
        }

        public string To
        {
            get => _to;
        }

        public int Armies
        {
            get => _armies;
        }

        public int AttackTurn
        {
            get => _attackTurn;
        }
    }
}