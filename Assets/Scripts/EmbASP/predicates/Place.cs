using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("place")]
    public class Place
    {
        [Param(0)] private int _turn;

        [Param(1)] private string _player;

        [Param(2)] private string _territory;

        [Param(3)] private int _armies;
    }
}