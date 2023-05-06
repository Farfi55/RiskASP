using Actions;
using UnityEngine;

namespace UI
{
    public class NextTurnPhaseButton : MonoBehaviour
    {
        public void NextTurnPhase()
        {
            var gm = GameManager.Instance;
            var endPhaseAction = new EndPhaseAction(gm.CurrentPlayer, gm.Turn);
            ActionReader.Instance.AddAction(endPhaseAction);
        }
    }
}