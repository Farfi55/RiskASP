using Actions;
using it.unical.mat.embasp.languages.asp;
using Map;
using player;

namespace TurnPhases.AI
{
    public class FortifyAIPhase : IAIPhase
    {
        private readonly GameManager _gm;
        private readonly ActionReader _ar;
        
        private FortifyPhase _fortifyPhase => _gm.FortifyPhase;
        

        public FortifyAIPhase(GameManager gm, ActionReader ar)
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