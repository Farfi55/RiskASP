using System;
using Actions;
using EmbASP.predicates;
using Extensions;
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

        public void OnPhaseStart()
        {
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
                    action = new FortifyAction(player, _gm.Turn, _tr.FromName(move.From.StripQuotes()),
                        _tr.FromName(move.To.StripQuotes()), move.Troops);
                }

                if (action != null)
                    _ar.AddAction(action);
            }
        }

        public void OnFailure(Player player)
        {
            _ar.AddAction(new EndPhaseAction(player, _gm.Turn));
        }

        public void OnPhaseEnd()
        {
        }
    }
}