using System;
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
        
        private FortifyPhase _fortifyPhase => _gm.FortifyPhase;
        

        public FortifyAIPhase(GameManager gm, ActionReader ar)
        {
            _gm = gm;
            _ar = ar;
        }

        public void Start(InputProgram inputProgram)
        {
            inputProgram.AddObjectInput(new EmbASP.predicates.Player(_gm.CurrentPlayer.Name));
            foreach (var territory in _gm.TerritoryRepository.Territories.Values)
            {
                inputProgram.AddObjectInput(new TerritoryControl(_gm.Turn,territory.Name,territory.Owner.Name,territory.Troops));
            }
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
                        var fortifyAction = new FortifyAction( _gm.CurrentPlayer,_gm.Turn,_gm.TerritoryRepository.Territories[move.From], _gm.TerritoryRepository.Territories[move.To], move.Armies);
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
            throw new NotImplementedException();
        }
    }
}