using Actions;
using player;

namespace TurnPhases
{
    public interface IPhase
    {
        string Name { get; }
        void Start(Player player);
        void OnAction(Player player, PlayerAction action);
        
        void End(Player player);

    }
}