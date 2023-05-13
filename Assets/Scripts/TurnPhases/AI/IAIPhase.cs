using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;

namespace TurnPhases.AI
{
    public interface IAIPhase
    {
        
        void OnPhaseStart();
        
        void OnPhaseEnd();

        void OnRequest(player.Player player, InputProgram inputProgram); 

        void OnResponse(player.Player player, AnswerSet answerSet);
        
        void OnFailure(player.Player player);
        
    }
}