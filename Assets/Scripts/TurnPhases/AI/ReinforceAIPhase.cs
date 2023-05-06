using System;
using Actions;
using EmbASP.predicates;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using Map;
using Player = player.Player;
using EmbASP.predicates;
namespace TurnPhases.AI
{
    public class ReinforceAIPhase : IAIPhase
    {
        private readonly GameManager _gm;
        private readonly ActionReader _ar;
        private readonly TerritoryRepository _tr;
        private readonly Player _pl;
        private int _remainingTroopsToPlace;
        
        private ReinforcePhase _reinforcePhase => _gm.ReinforcePhase;
        
        

        public ReinforceAIPhase(GameManager gm, TerritoryRepository tr, ActionReader ar, Player pl)
        {
            _gm = gm;
            _ar = ar;
            _tr = tr;
            _pl = pl;
        }
        
        
        public void Start(InputProgram inputProgram)
        {
            _remainingTroopsToPlace = _pl.GetTotalTroopBonus();
            if (_remainingTroopsToPlace == 0) _gm.NextTurnPhase();
            
            //The player name is inside the UnitsToPlace predicate
            inputProgram.AddObjectInput(new UnitsToPlace(_gm.Turn,_pl.Name,_remainingTroopsToPlace));
            foreach (var territory in _tr.Territories.Values)
            {
                inputProgram.AddObjectInput(new TerritoryControl(_gm.Turn,territory.Name,territory.Owner.Name,territory.Troops));
            }
        }

        public void OnResponse(AnswerSet answerSet)
        {
            //TODO: add the ReinforceTerritory class in the ASPMapper along with the others

            try
            {
                foreach (var obj in answerSet.Atoms)
                {
                    if (obj is ReinforceTerritory)
                    {
                        var reinforceTerritory = (ReinforceTerritory) obj;
                        var reinforceAction = new ReinforceAction(_pl,_gm.Turn,_tr.Territories[reinforceTerritory.Territory], reinforceTerritory.Number);
                        
                        _ar.AddAction(reinforceAction);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void End(Player player)
        {
            throw new System.NotImplementedException();
        }
    }
}