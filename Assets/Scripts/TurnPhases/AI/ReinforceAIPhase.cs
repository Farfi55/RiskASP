using Actions;
using it.unical.mat.embasp.languages.asp;
using Map;
using player;

namespace TurnPhases.AI
{
    public class ReinforceAIPhase : IAIPhase
    {
        private readonly GameManager _gm;
        private readonly ActionReader _ar;
        
        private ReinforcePhase _reinforcePhase => _gm.ReinforcePhase;
        

        public ReinforceAIPhase(GameManager gm, ActionReader ar)
        {
            _gm = gm;
            _ar = ar;
        }

        public void Start(Player player)
        {
            throw new System.NotImplementedException();
        }

        public void OnResponse(AnswerSet answerSet)
        {
            throw new System.NotImplementedException();
        }

        public void End(Player player)
        {
            throw new System.NotImplementedException();
        }
    }
}