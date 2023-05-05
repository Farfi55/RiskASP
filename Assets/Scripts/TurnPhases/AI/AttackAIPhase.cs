using System;
using Actions;
using EmbASP;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using Map;
using player;
using UnityEngine;

namespace TurnPhases.AI
{
    public class AttackAIPhase : IAIPhase
    {
        private readonly GameManager _gm;
        private readonly ActionReader _ar;
        private readonly TerritoryRepository _tr;
        
        

        private AttackPhase _attackPhase => _gm.AttackPhase;
        public AttackState State => _attackPhase.State;

        
        public AttackAIPhase(GameManager gm, ActionReader ar, TerritoryRepository tr)
        {
            _gm = gm;
            _ar = ar;
            _tr = tr;
        }

        public void Start(Player player)
        {
            
            AIController controller = new AIController();
            controller.ConfigAsp();
            controller.StartRenforcement(player, TerritoryRepository.Instance);
        }

        public void OnResponse(AnswerSet answerSet)
        {
            throw new NotImplementedException();
        }


        public void End(Player player)
        {
            throw new NotImplementedException();
        }
    }
}