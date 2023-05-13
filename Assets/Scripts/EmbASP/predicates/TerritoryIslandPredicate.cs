using System.Collections.Generic;
using System.Linq;
using it.unical.mat.embasp.languages;
using Map;

namespace EmbASP.predicates
{
    
    [Id("territory_island")]
    public class TerritoryIslandPredicate
    {
        [Param(0)] public int Turn;

        [Param(1)] public int Id;

        [Param(2)] public string Territory;

        [Param(3)] public string Player;

        public TerritoryIslandPredicate()
        {
        }
        
        public TerritoryIslandPredicate(int turn, int id, string territory, string player)
        {
            Turn = turn;
            Id = id;
            Territory = territory;
            Player = player;
        }


        public int setTurn(int turn) => Turn = turn;
        public int setId(int id) => Id = id;
        public string setTerritory(string territory) => Territory = territory;
        public string setPlayer(string player) => Player = player;
        
        public int getTurn() => Turn;
        public int getId() => Id;
        public string getTerritory() => Territory;
        public string getPlayer() => Player;
    }
    
}