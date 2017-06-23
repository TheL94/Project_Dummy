using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.StateMachine {
    public abstract class StateBehaviour
    {
        public virtual void OnStart() { }
        public virtual void OnRun() { }
        public virtual void OnEnd() { }

        public void Start()
        {
            OnStart();
        }
        public void Run()
        {
            OnRun();
        }
        public void End()
        {
            OnEnd();
        }
    }
}
