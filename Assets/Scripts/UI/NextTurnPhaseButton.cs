using Actions;
using UnityEngine;

namespace UI
{
    public class NextTurnPhaseButton : MonoBehaviour
    {
        public void NextTurnPhase()
        {
            var endPhaseAction = new EndPhaseAction(GameManager.Instance.CurrentPlayer);
            ActionReader.Instance.AddAction(endPhaseAction);
        }
    }
}