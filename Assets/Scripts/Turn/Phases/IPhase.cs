using map;
using player;

namespace Turn.Phases
{
    public interface IPhase
    {
        string Name { get; }
        void Start(Player player);
        void OnAction(Player player, PlayerAction action);
        
        void End(Player player);

    }
}