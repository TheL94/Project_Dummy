using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    public abstract class Decision : ScriptableObject
    {
        public abstract int Decide(AIController _controller);
    }
}