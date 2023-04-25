using Actions;
using Map;
using player;

namespace TurnPhases.Selection
{
    public interface ISelectionPhase
    {
        void Start(Player player);
        
        void OnSelected(Player player, TerritorySelection selection);
        
        void OnUnselected(Player player, TerritorySelection selection);
        
        void End(Player player);
    }
}