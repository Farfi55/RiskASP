using System;
using Actions;
using Map;
using player;
using UnityEngine;

namespace TurnPhases
{
    public class FortifyPhase : IPhase
    {
        public string Name => "Fortify";
        
        private readonly GameManager _gm;
        private readonly ContinentRepository _cr;
        private readonly TerritoryRepository _tr;

        public Action<FortifyAction> OnFortifyAction;


        public FortifyPhase(GameManager gameManager, ContinentRepository continentRepository,
            TerritoryRepository territoryRepository)
        {
            _gm = gameManager;
            _cr = continentRepository;
            _tr = territoryRepository;
        }

        public void Start(Player player)
        {
        }

        public void OnAction(Player player, PlayerAction action)
        {
            if (action is FortifyAction fortifyAction)
            {
                fortifyAction.From.RemoveTroops(fortifyAction.MovedTroops);
                fortifyAction.To.AddTroops(fortifyAction.MovedTroops);
                OnFortifyAction?.Invoke(fortifyAction);
                _gm.NextTurnPhase();
            }
            else if(action is EndPhaseAction)
            {
                _gm.NextTurnPhase();
            }
            else
                Debug.LogWarning($"FortifyPhase: Received action of type {action.GetType().Name}");
        }


        public void End(Player player)
        {
        }
        
    }
}