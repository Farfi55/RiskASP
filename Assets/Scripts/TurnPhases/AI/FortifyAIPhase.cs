using Actions;
using EmbASP.predicates;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using Map;
using Player = player.Player;
using UnityEngine;

namespace TurnPhases.AI
{
    public class FortifyAIPhase : IAIPhase
    {
        private readonly GameManager _gm;
        private readonly ActionReader _ar;
        private readonly TerritoryRepository _tr;
        private readonly Player _pl;
        
        private FortifyPhase _fortifyPhase => _gm.FortifyPhase;
        

        public FortifyAIPhase(GameManager gm, ActionReader ar)
        {
            _gm = gm;
            _ar = ar;
        }

        public void Start(InputProgram inputProgram)
        {
            Debug.Log("FortifyAIPhase Start");
        }

        public void OnResponse(AnswerSet answerSet)
        {
            try
            {
                foreach (var obj in answerSet.Atoms)
                {
                    if (obj is Move)
                    {
                        var move = (Move) obj;
                        var fortifyAction = new FortifyAction(_pl,_tr.Territories[move.From], _tr.Territories[move.To], move.Armies);
                        _ar.AddAction(fortifyAction);
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
            inputProgram.AddObjectInput(_pl.Name);
            foreach (var territory in _tr.Territories.Values)
            {
                inputProgram.AddObjectInput(new TerritoryControl(_gm.Turn,territory.Name,territory.Owner.Name,territory.Troops));
            }
        }
    }
}