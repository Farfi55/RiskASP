using System;
using JetBrains.Annotations;
using Map;

namespace Cards
{
    [Serializable]
    public class Card
    {
        public CardType Type;
        [CanBeNull] public Territory Territory;

        public readonly string Name;

        public Card(CardType type, [CanBeNull] Territory territory)
        {
            Type = type;
            Territory = territory;
            Name = territory != null ? territory.Name : type.ToString();
            
            if(type == CardType.Wild && territory != null)
                throw new ArgumentException("Wild card cannot have a territory");

            if (type != CardType.Wild && territory == null)
                throw new ArgumentException("Non-wild card must have a territory");

        }
    }
}