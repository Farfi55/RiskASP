using Actions;
using player;

namespace TurnPhases
{
    public class EmptyPhase : IPhase
    {
        public string Name => "Empty";
        
        public void Start(Player player)
        {
        }

        public void OnAction(Player player, PlayerAction action)
        {
        }

        public void End(Player player)
        {
        }
    }
}