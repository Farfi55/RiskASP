using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("territory_control")]
    public class TerritoryControl
    {
        [Param(0)] private int _turn;

        [Param(1)] private string _territory;

        [Param(2)] private string _player;

        [Param(3)] private int _armies;

        public TerritoryControl(int turn, string territory, string player, int armies)
        {
            _turn = turn;
            _territory = territory;
            _player = player;
            _armies = armies;
        }
    }
    
}