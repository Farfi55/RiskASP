using System;
using Actions;
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

        private AttackPhase _attackPhase => _gm.AttackPhase;
        public AttackState State => _attackPhase.State;

        
        public AttackAIPhase(GameManager gm, ActionReader ar)
        {
            _gm = gm;
            _ar = ar;
        }

        public void Start(Player player)
        {
            throw new NotImplementedException();
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