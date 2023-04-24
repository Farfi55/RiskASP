using System.Net;
using System.Runtime.InteropServices;
using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("move")]
    public class Move
    {
        [Param(0)] private int _turn;

        [Param(1)] private string _from;

        [Param(2)] private string _to;

        [Param(3)] private int _armies;
    }
}