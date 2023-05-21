using System;
using System.Collections.Generic;
using Actions;
using Cards;
using EmbASP.predicates;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using Map;
using Player = player.Player;
using Extensions;
using UnityEngine;

namespace TurnPhases.AI
{
    public class ReinforceAIPhase : IAIPhase
    {
        private readonly GameManager _gm;
        private readonly ActionReader _ar;
        private readonly TerritoryRepository _tr;
        private readonly CardRepository _cr;

        private ReinforcePhase _reinforcePhase => _gm.ReinforcePhase;


        public ReinforceAIPhase(GameManager gm, ActionReader ar, TerritoryRepository tr, CardRepository cr)
        {
            _gm = gm;
            _ar = ar;
            _tr = tr;
            _cr = cr;
        }


        public void OnPhaseStart()
        {
        }

        public void OnRequest(Player player, InputProgram inputProgram)
        {
            var troopsToPlace = _reinforcePhase.RemainingTroopsToPlace;
            inputProgram.AddObjectInput(new TroopsToPlacePredicate(_gm.Turn, player.Name, troopsToPlace));

            for (var exchangeId = 0; exchangeId < player.BestCardExchangeCombinations.Count; exchangeId++)
            {
                AddPossibleCardExchange(player, inputProgram, exchangeId);
            }
        }

        private void AddPossibleCardExchange(Player player, InputProgram inputProgram, int exchangeId)
        {
            var exchange = player.BestCardExchangeCombinations[exchangeId];
            var exchangeTypeIndex = _cr.GetExchangeTypeIndex(exchange.ExchangeType);
            if (exchangeTypeIndex == -1)
                throw new Exception("Invalid card combination");

            var cardName1 = exchange.Cards[0].Name;
            var cardName2 = exchange.Cards[1].Name;
            var cardName3 = exchange.Cards[2].Name;

            var exchangePredicate = new PossibleCardExchangePredicate(_gm.Turn, player.Name,
                exchangeId, exchangeTypeIndex, 
                cardName1, cardName2, cardName3, exchange.ExchangeValue);
            inputProgram.AddObjectInput(exchangePredicate);
        }

        public void OnResponse(Player player, AnswerSet answerSet)
        {
            int remainingTroopsToPlace = _reinforcePhase.RemainingTroopsToPlace;
            int troopsDrafted = 0;

            var reinforceActions = new List<ReinforceAction>();
            ExchangeCardsAction exchangeCardsAction = null;

            foreach (var atom in answerSet.Atoms)
            {
                if (atom is DraftPredicate draft)
                {
                    var territory = _tr.FromName(draft.Territory.StripQuotes());
                    reinforceActions.Add(new ReinforceAction(player, draft.Turn, territory, draft.Troops));
                    troopsDrafted += draft.Troops;
                }
                else if (atom is ExchangeCardsPredicate exchangeCardsPredicate)
                {
                    var exchange = player.BestCardExchangeCombinations[exchangeCardsPredicate.ExchangeId];
                    remainingTroopsToPlace += exchange.ExchangeValue;
                    exchangeCardsAction = new ExchangeCardsAction(player, _gm.Turn, exchange);
                }
            }

            if (exchangeCardsAction != null)
                _ar.AddAction(exchangeCardsAction);
            foreach (var reinforceAction in reinforceActions)
            {
                _ar.AddAction(reinforceAction);
            }


            if (troopsDrafted != remainingTroopsToPlace)
                Debug.LogError(
                    $"The AI drafted {troopsDrafted} troops, but it should have drafted {remainingTroopsToPlace} troops");
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