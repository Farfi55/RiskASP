using Actions;
using EmbASP.predicates;
using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using Map;
using Player = player.Player;

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

        public void Start(Player player, InputProgram inputProgram)
        {
            // inputProgram.AddObjectInput(new Phase("reinforce"));
            // inputProgram.AddObjectInput(new UnitsToPlace(_reinforcePhase.RemainingTroopsToPlace));
            
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