using System;
using UnityEngine;

namespace player
{
    public abstract class PlayerConfiguration: ScriptableObject
    {
        public virtual string Name { get; }
    }
}