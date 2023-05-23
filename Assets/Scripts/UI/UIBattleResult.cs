using System;
using TurnPhases;
using UnityEngine;

namespace UI
{
    public class UIBattleResult : MonoBehaviour
    {
        private GameManager _gm;
        
        [SerializeField] private SpriteRenderer[] _attackDiceRenderers;
        [SerializeField] private SpriteRenderer[] _defenderDiceRenderers;
        [SerializeField] private SpriteRenderer[] _battleResultRenderers;
        
        [Space(10)]
        [SerializeField] private Sprite[] _diceSprites;

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
                _attackDiceRenderers[index].color = _attackerColor;
                _defenderDiceRenderers[index].color = _defenderColor;
                
                _attackDiceRenderers[index].sprite = null;
                _defenderDiceRenderers[index].sprite = null;
            }
            
            for (var index = 0; index < attackResult.AttackingTroops; index++)
            {
                var attackerRoll = attackResult.AttackerRolls[index];
                _attackDiceRenderers[index].sprite = _diceSprites[attackerRoll - 1];
            }
            
            for (var index = 0; index < attackResult.DefendingTroops; index++)
            {
                var defenderRoll = attackResult.DefenderRolls[index];
                _defenderDiceRenderers[index].sprite = _diceSprites[defenderRoll - 1];
            }

            var battleWidth = Math.Min(attackResult.AttackingTroops, attackResult.DefendingTroops);
            for (var index = 0; index < battleWidth; index++)
            {
                var attackerRoll = attackResult.AttackerRolls[index];
                var defenderRoll = attackResult.DefenderRolls[index];
                bool attackerWon = attackerRoll > defenderRoll;

                if (attackerWon)
                {
                    _attackDiceRenderers[index].color = _attackerWonColor;
                }
                else
                {
                    _defenderDiceRenderers[index].color = _defenderWonColor;
                }
            }
        }
    }
}