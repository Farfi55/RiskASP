using Actions;
using Map;
using player;

namespace TurnPhases.Selection
{
    public interface ISelectionPhase
    {
        void Start(Player player);
        
        void OnClicked(Player player, TerritorySelection selection);

        void End(Player player);
    }
}