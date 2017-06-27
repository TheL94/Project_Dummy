using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(AIController _controller);
    }
}
