using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("units_to_place")]
    public class UnitsToPlace
    {
        [Param(0)] private int _turn;
        
        [Param(1)] private string _player;

        [Param(2)] private int _number;
    }
}