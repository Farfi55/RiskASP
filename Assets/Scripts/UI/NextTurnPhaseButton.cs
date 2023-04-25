using UnityEngine;

namespace UI
{
    public class NextTurnPhaseButton : MonoBehaviour
    {
        public void NextTurnPhase() => GameManager.Instance.NextTurnPhase();
    }
}
