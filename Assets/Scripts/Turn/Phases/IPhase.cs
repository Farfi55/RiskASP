using map;
using player;

namespace Turn.Phases
{
    public interface IPhase
    {
        void Start(Player player);
        void OnAction(Player player, PlayerAction action);
        
        void End(Player player);

    }
}