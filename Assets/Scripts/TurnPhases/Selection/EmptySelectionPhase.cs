using Actions;
using Map;
using player;

namespace TurnPhases.Selection
{
    public class EmptySelectionPhase : ISelectionPhase
    {
        public void Start(Player player)
        {
        }

        public void OnClicked(Player player, TerritorySelection selection)
        {
        }

        public void OnUnselected(Player player, TerritorySelection selection)
        {
        }

        public void End(Player player)
        {
        }
    }
}