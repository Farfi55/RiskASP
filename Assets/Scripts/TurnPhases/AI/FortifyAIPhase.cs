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
        private readonly TerritoryRepository _tr;

        private FortifyPhase _fortifyPhase => _gm.FortifyPhase;


        public FortifyAIPhase(GameManager gm, ActionReader ar, TerritoryRepository tr)
        {
            _gm = gm;
            _ar = ar;
            _tr = tr;
        }

        public void OnRequest(Player player, InputProgram inputProgram)
        {
        }

        public void OnResponse(Player player, AnswerSet answerSet)
        {
            foreach (var atom in answerSet.Atoms)
            {
                PlayerAction action = null;
                if (atom is FortifyPredicate move)
                {
                    action = new FortifyAction(player, _gm.Turn, _tr.FromName(move.From),
                        _tr.FromName(move.From), move.Troops);
                }

                if (action != null)
                    _ar.AddAction(action);
            }
        }

        public void End(Player player)
        {
        }
    }
}