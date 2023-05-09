using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using player;

namespace TurnPhases.AI
{
    public interface IAIPhase
    {
        
        void OnRequest(player.Player player, InputProgram inputProgram); 

        void OnResponse(player.Player player, AnswerSet answerSet);

        void End(Player player);
    }
}