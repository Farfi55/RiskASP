using System;
using TurnPhases;
using UnityEngine;

namespace player
{
    [RequireComponent(typeof(Player))]
    public class BotPlayer : MonoBehaviour
    {
        public BotBrain botBrain => _botBrain;
        [SerializeField] private BotBrain _botBrain;
        private Player _player;
        private GameManager _gm;


        private void Awake()
        {
            _player = GetComponent<Player>();
            if (_botBrain == null)
            {
                Debug.Log("BotPlayer Awake: BotBrain is null, finding a BotBrain in the scene");
                _botBrain = FindObjectOfType<BotBrain>();
            }
        }

        private void OnEnable()
        {
            _gm = GameManager.Instance;
            _gm.OnTurnPhaseChanged += OnTurnPhaseChanged;
            _gm.AttackPhase.OnAttacked += OnAttacked;
        }

        private void OnAttacked(AttackResult attackResult)
        {
            if (!_gm.IsCurrentPlayer(_player))
                return;
            
            _botBrain.HandleCommunication(_player);

        }

        private void OnTurnPhaseChanged(IPhase oldPhase, IPhase newPhase)
        {
            if (!_gm.IsCurrentPlayer(_player) || _gm.GamePhase != GamePhase.Playing)
                return;
            
            _botBrain.OnTurnPhaseChanged(oldPhase, newPhase);
            _botBrain.HandleCommunication(_player);
        }
        
        
    }
}