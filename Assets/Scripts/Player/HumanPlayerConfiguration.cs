using UnityEngine;

namespace player
{
    [CreateAssetMenu(fileName = "HumanConfiguration", menuName = "HumanConfiguration", order = 1)]
    public class HumanPlayerConfiguration : PlayerConfiguration
    {
        public override string Name => _name;
        [SerializeField] private string _name;
    }
}