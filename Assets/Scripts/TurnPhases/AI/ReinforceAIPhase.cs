using System;
using Actions;
using EmbASP.predicates;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using Map;
using Player = player.Player;
using EmbASP.predicates;
using Extensions;
using UnityEngine;

namespace TurnPhases.AI
{
    public class ReinforceAIPhase : IAIPhase
    {
        private readonly GameManager _gm;
        private readonly ActionReader _ar;
        private readonly TerritoryRepository _tr;

        private ReinforcePhase _reinforcePhase => _gm.ReinforcePhase;


        public ReinforceAIPhase(GameManager gm, ActionReader ar, TerritoryRepository tr)
        {
            _gm = gm;
            _ar = ar;
            _tr = tr;
        }


        public void OnRequest(Player player, InputProgram inputProgram)
        {
            var troopsToPlace = _reinforcePhase.RemainingTroopsToPlace;
            inputProgram.AddObjectInput(new TroopsToPlacePredicate(_gm.Turn, player.Name, troopsToPlace));
        }

        public void OnResponse(Player player, AnswerSet answerSet)
        {
            int troopsDrafted = 0;

            foreach (var atom in answerSet.Atoms)
            {
                PlayerAction action = null;

                if (atom is DraftPredicate draft)
                {
                    var territory = _tr.FromName(draft.Territory.StripQuotes());
                    action = new ReinforceAction(player, _gm.Turn, territory, draft.Troops);
                    troopsDrafted += draft.Troops;
                }

                if (action != null)
                    _ar.AddAction(action);
            }

            int remainingTroopsToPlace = _reinforcePhase.RemainingTroopsToPlace;
            if (troopsDrafted != remainingTroopsToPlace)
                Debug.LogError(
                    $"The AI drafted {troopsDrafted} troops, but it should have drafted {remainingTroopsToPlace} troops");
        }

        public void End(Player player)
        {
        }
    }
}