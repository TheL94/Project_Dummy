using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    [Serializable]
    public class Transition
    {
        public Decision Decision;
        public State[] NextPosiibleStates;
    }
}