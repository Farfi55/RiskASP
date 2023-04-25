using UnityEngine;
using UnityEngine.Serialization;

namespace player
{
    [CreateAssetMenu(menuName = "Create PlayerColor", fileName = "PlayerColor")]
    public class PlayerColor : ScriptableObject
    {
        public Color Disabled;
        public Color Normal;
        public Color Highlight;
        public Color Selected;
    }
}