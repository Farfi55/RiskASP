using System;
using player;
using TMPro;
using Turn.Phases;
using UnityEngine;

namespace UI
{
    public class UIGameInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playerTurnText;
        [SerializeField] private TMP_Text _turnPhaseText;
        [SerializeField] private TMP_Text _turnText;
        [SerializeField] private RectTransform[] _extraInfo;
        private TMP_Text[] _extraInfoTexts;
        
        private GameManager _gm;


        private void Awake()
        {
            _extraInfoTexts = new TMP_Text[_extraInfo.Length];
            for (var i = 0; i < _extraInfo.Length; i++)
            {
                _extraInfoTexts[i] = _extraInfo[i].GetComponentInChildren<TMP_Text>();
            }
        }

        private void Start()
        {
            _gm = GameManager.Instance;
            _gm.OnPlayerTurnChanged += OnPlayerTurnChanged;
            _gm.OnTurnPhaseChanged += OnTurnPhaseChanged;
            
            _gm.ReinforcePhase.OnTroopsToPlaceChanged += () =>
            {
                if (_gm.CurrentPhase is ReinforcePhase reinforcePhase)
                    DisplayReinforceInfo(reinforcePhase);
            };
            
            UpdatePlayerText(_gm.CurrentPlayer);
            UpdateTurnText(_gm.Turn);
            UpdateTurnPhaseText(_gm.CurrentPhase);
        }


        private void UpdatePlayerText(Player player) => _playerTurnText.text = $"{player.Name}'s Turn";
        private void UpdateTurnText(int turn) => _turnText.text = $"Turn {turn}";
        private void UpdateTurnPhaseText(IPhase phase)
        {
            _turnPhaseText.text = $"{phase.Name} Phase";
            DisplayExtraInfo(phase);
        }

        private void OnTurnPhaseChanged(IPhase oldPhase, IPhase newPhase)
        {
            UpdateTurnPhaseText(newPhase);
        }

        private void DisplayExtraInfo(IPhase currentPhase)
        {
            foreach (var extraInfoTransform in _extraInfo) extraInfoTransform.gameObject.SetActive(false);

            switch (currentPhase)
            {
                case ReinforcePhase reinforcePhase:
                    DisplayReinforceInfo(reinforcePhase);
                    break;
                case AttackPhase attackPhase:
                    DisplayAttackInfo(attackPhase);
                    break;
            }
        }

        private void DisplayReinforceInfo(ReinforcePhase reinforcePhase)
        {
            _extraInfo[0].gameObject.SetActive(true);
            _extraInfoTexts[0].text = $"Reinforcements: {reinforcePhase.RemainingTroopsToPlace}";
        } 
        
        private void DisplayAttackInfo(AttackPhase attackPhase)
        {
            throw new NotImplementedException();
        }
        
        private void OnPlayerTurnChanged(Player oldPlayer, Player newPlayer)
        {
            UpdatePlayerText(newPlayer);
            UpdateTurnText(_gm.Turn);
        }
    }
}