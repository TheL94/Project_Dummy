using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.SM {
    public abstract class StateBase
    {

        public abstract void Start();
        public abstract void Run();
        public abstract void End();
    }
}
