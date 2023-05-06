using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("reinforce_territory")]
    public class ReinforceTerritory
    {
        [Param(0)] private int _number;

        [Param(1)] private string _territory;

        public ReinforceTerritory(int number, string territory)
        {
            _number = number;
            _territory = territory;
        }

        public int Number
        {
            get => _number;
        }

        public string Territory
        {
            get => _territory;
        }
    }
}