using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using player;

namespace TurnPhases.AI
{
    public interface IAIPhase
    {
        
        void Start(InputProgram inputProgram); 

        void OnResponse(AnswerSet answerSet);

        void End(Player player);
    }
}