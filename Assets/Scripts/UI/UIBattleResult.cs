using System;
using TurnPhases;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIBattleResult : MonoBehaviour
    {
        private GameManager _gm;
        
        [SerializeField] private Image[] _attackDiceRenderers;
        [SerializeField] private Image[] _defenderDiceRenderers;
        [SerializeField] private Image[] _battleResultRenderers;
        
        [Space(10)]
        [SerializeField] private Sprite[] _diceSprites;

        [SerializeField] private Sprite _attackerWonSprite;
        [SerializeField] private Sprite _defenderWonSprite;
        
        
        [Space(10)]
        [SerializeField] private Color _attackerColor;
        [SerializeField] private Color _attackerWonColor;
        
        [Space(10)]
        [SerializeField] private Color _defenderColor;
        [SerializeField] private Color _defenderWonColor;


        private void Start()
        {
            _gm = GameManager.Instance;
            _gm.AttackPhase.OnAttacked += (attackResult) =>
            {
                if (_gm.CurrentPhase is AttackPhase attackPhase)
                    DisplayAttackResult(attackPhase, attackResult);
            };
        }

        private void DisplayAttackResult(AttackPhase attackPhase, AttackResult attackResult)
        {
            for (int index = 0; index < 3; index++)
            {
                var attackDiceRenderer = _attackDiceRenderers[index];
                var defenderDiceRenderer = _defenderDiceRenderers[index];
                
                attackDiceRenderer.color = _attackerColor;
                defenderDiceRenderer.color = _defenderColor;

                attackDiceRenderer.enabled = false;
                defenderDiceRenderer.enabled = false;
                _battleResultRenderers[index].enabled = false;
                
                attackDiceRenderer.sprite = null;
                defenderDiceRenderer.sprite = null;
            }
            
            for (var index = 0; index < attackResult.AttackingTroops; index++)
            {
                var attackerRoll = attackResult.AttackerRolls[index];
                _attackDiceRenderers[index].sprite = _diceSprites[attackerRoll - 1];
                _attackDiceRenderers[index].enabled = true;
            }
            
            for (var index = 0; index < attackResult.DefendingTroops; index++)
            {
                var defenderRoll = attackResult.DefenderRolls[index];
                _defenderDiceRenderers[index].sprite = _diceSprites[defenderRoll - 1];
                _defenderDiceRenderers[index].enabled = true;
            }

            var battleWidth = Math.Min(attackResult.AttackingTroops, attackResult.DefendingTroops);
            for (var index = 0; index < battleWidth; index++)
            {
                var attackerRoll = attackResult.AttackerRolls[index];
                var defenderRoll = attackResult.DefenderRolls[index];
                bool attackerWon = attackerRoll > defenderRoll;

                _battleResultRenderers[index].enabled = true;
                
                if (attackerWon)
                {
                    _attackDiceRenderers[index].color = _attackerWonColor;
                    _battleResultRenderers[index].sprite = _attackerWonSprite;
                }
                else
                {
                    _defenderDiceRenderers[index].color = _defenderWonColor;
                    _battleResultRenderers[index].sprite = _defenderWonSprite;
                }
            }
        }
    }
}