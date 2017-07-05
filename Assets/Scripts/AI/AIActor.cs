using Framework.AI;
using Framework.Pathfinding;
using DumbProject.Grid;
using DumbProject.Rooms;
using DumbProject.Items;
using UnityEngine;
using DG.Tweening;

namespace DumbProject.Generic {
    public class AIActor : AIController
    {
        public float LookDuration;
        public float MoveDuration;
        public float InteractionRadius;

        public Tweener pathTracking { get; protected set; }
        /// <summary>
        /// Actual position onto the INetworkable grid. 
        /// Can't be Set. Any try will change nothing.
        /// </summary>
        public override INetworkable CurrentNode
        {
            get
            {
                return grid.GetSpecificGridNode(transform.position);
            }
            set
            {
                Debug.LogWarning("Can't Set CurrentNode fild in Dumby");
                return;
            }
        }

        protected Animator animator;
        AnimationState _animState = AnimationState.Idle;
        public AnimationState AnimState
        {
            get { return _animState; }
            set
            {
                _animState = value;
                animator.SetInteger("AnimationState", (int)AnimState);
            }
        }

        protected GridController grid;
        public Cell CurrentCell { get { return grid.GetSpecificGridNode(transform.position).RelativeCell; } }
        public Room CurrentRoom { get { return CurrentCell.RelativeRoom; } }

        public IDroppable nextRoomObjective;

        public void FollowPath()
        {
            AnimState = AnimationState.Running;
            if (nodePath == null || nodePath.Count <= 0)
                return;
            if (Vector3.Distance(nodePath[0].spacePosition, transform.position) <= InteractionRadius)
                nodePath.Remove(nodePath[0]);
            if (nodePath.Count == 0)
                return;

            if (pathTracking == null)
            {
                pathTracking = transform.DOLookAt(nodePath[0].spacePosition, LookDuration).OnComplete(() =>
                {
                    pathTracking = transform.DOMove(nodePath[0].spacePosition, MoveDuration);
                });
            }
            else
            {
                if (Vector3.Distance(transform.position, nodePath[0].spacePosition) <= InteractionRadius)
                {
                    nodePath.Remove(nodePath[0]);
                    pathTracking.Complete();
                    pathTracking = null;
                }
            }
        }

    }

    public enum AnimationState
    {
        Fallen = 0,
        Idle = 1,
        Running = 2,
        Fighting = 3
    }
}
