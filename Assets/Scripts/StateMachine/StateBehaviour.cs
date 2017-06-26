using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.StateMachine {
    public abstract class StateBehaviour
    {
        public bool IsStarted = false;
        public virtual void OnStart() { }
        public virtual void OnRun() { }
        public virtual void OnEnd() { }

        public void Start()
        {
            OnStart();
            IsStarted = true;
        }
        public void Run()
        {
            if (IsStarted)
                OnRun();
            else
                Start();
        }
        public void End()
        {
            OnEnd();
            IsStarted = false;
        }
    }
}
