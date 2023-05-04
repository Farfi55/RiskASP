using it.unical.mat.embasp.languages.asp;
using player;

namespace TurnPhases.AI
{
    public interface IAIPhase
    {
        void Start(Player player);

        void OnResponse(AnswerSet answerSet);

        void End(Player player);
    }
}