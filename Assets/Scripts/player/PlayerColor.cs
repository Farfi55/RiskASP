using UnityEngine;
using UnityEngine.Serialization;

namespace player
{
    [CreateAssetMenu(menuName = "Create PlayerColor", fileName = "PlayerColor")]
    public class PlayerColor : ScriptableObject
    {
        public Color Normal;
        [FormerlySerializedAs("Selected")] public Color Highlight;
        public Color Disabled;
    }
}