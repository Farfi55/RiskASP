using System;
using TurnPhases;
using UnityEngine;

namespace player
{
    [RequireComponent(typeof(Player))]
    public class BotPlayer : MonoBehaviour
    {
        public BotBrain botBrain { get; private set; }
        private Player _player;
        private GameManager _gm;


        private void Awake()
        {
            _gm = GameManager.Instance;
            
            _player = GetComponent<Player>();
            
            _gm.OnTurnPhaseChanged += OnTurnPhaseChanged;
            _gm.AttackPhase.OnAttacked += OnAttacked;
        }

        private void OnAttacked(AttackResult attackResult)
        {
            if (!_gm.IsCurrentPlayer(_player))
                return;
            
            botBrain.HandleCommunication(_player);

        }

        private void OnTurnPhaseChanged(IPhase oldPhase, IPhase newPhase)
        {
            
        }


        private void StartTurn()
        {
            botBrain.HandleCommunication(_player);
        }
    }
}