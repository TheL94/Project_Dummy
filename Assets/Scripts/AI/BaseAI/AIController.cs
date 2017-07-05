using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Pathfinding;

namespace Framework.AI
{
    public class AIController : MonoBehaviour
    {
        public State CurrentState;

        public bool DebugMode;
        public bool isActive = false;

        public virtual void Setup(bool _setActive = true)
        {
            CanPathfind = PathfinderSetup();
            isActive = _setActive;
        }

        private void Update()
        {
            if (!isActive)
                return;
            CurrentState.UpdateState(this);
            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        public void TransitionToState(State _nextState)
        {
            if (_nextState != CurrentState)
            {
                CurrentState = _nextState;
            }
        }

        private void OnDrawGizmos()
        {
            if (!DebugMode)
                return;
            if (CurrentState != null)
            {
                Gizmos.color = CurrentState.StateColor;
                Gizmos.DrawSphere(transform.position, 0.2f);
            }
        }


        #region Pathfinder
        public bool CanPathfind { get; protected set; }
        public virtual INetworkable CurrentNode { get; set; }
        public Pathfinder pathFinder;
        public List<INetworkable> nodePath = new List<INetworkable>();

        public bool PathfinderSetup()
        {
            if (CurrentNode != null)
            {
                pathFinder = new Pathfinder(CurrentNode);
                return true;
            }
            else
                return false;
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            foreach (INetworkable node in nodePath)
            {
                Gizmos.DrawCube(node.spacePosition, Vector3.one);
            }
        }
    }
}
