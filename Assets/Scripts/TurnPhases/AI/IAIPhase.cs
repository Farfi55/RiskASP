using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using player;

namespace TurnPhases.AI
{
    public interface IAIPhase
    {
        void Start(Player player, InputProgram inputProgram);

        void OnResponse(AnswerSet answerSet);

        void End(Player player);
    }
}