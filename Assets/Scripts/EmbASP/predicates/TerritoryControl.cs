using System.Collections.Generic;
using System.Linq;
using it.unical.mat.embasp.languages;
using Map;

namespace EmbASP.predicates
{
    [Id("territory_control")]
    public class TerritoryControl
    {
        [Param(0)] public int Turn;

        [Param(1)] public string Territory;

        [Param(2)] public string Player;

        [Param(3)] public int Troops;


        public TerritoryControl(int turn, string territory, string player, int troops)
        {
            Turn = turn;
            Territory = territory;
            Player = player;
            Troops = troops;
        }

        public TerritoryControl(int turn, Territory territory)
        {
            Turn = turn;
            Territory = territory.name;
            Player = territory.Owner.Name;
            Troops = territory.Troops;
        }

        public static ISet<TerritoryControl> FromTerritories(int turn, IList<Territory> territories)
        {
            ISet<TerritoryControl> territoryControls = new HashSet<TerritoryControl>(territories.Count());

            foreach (var territory in territories)
            {
                territoryControls.Add(new TerritoryControl(turn, territory));
            }

            return territoryControls;
        }
        public static ISet<object> FromTerritoriesAsObjects(int turn, IList<Territory> territories)
        {
            ISet<object> territoryControls = new HashSet<object>(territories.Count());

            foreach (var territory in territories)
            {
                territoryControls.Add(new TerritoryControl(turn, territory));
            }

            return territoryControls;
        }
    }
    
}