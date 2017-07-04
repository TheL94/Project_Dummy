using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms;
using DumbProject.Items;
using Framework.AI;
using Framework.Pathfinding;
using DG.Tweening;

namespace DumbProject.Generic
{
    public class Dumby : AIController
    {
        public float LookDuration;
        public float MoveDuration;
        public float InteractionRadius;
        public Tweener pathTracking { get; protected set; }

        Animator animator;
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
            set {
                Debug.LogWarning("Can't Set CurrentNode fild in Dumby");
                return; }
        }

        GridController grid;
        public Cell CurrentCell { get { return grid.GetSpecificGridNode(transform.position).RelativeCell; } }
        public Room CurrentRoom { get { return CurrentCell.RelativeRoom; } }

        public IDroppable nextRoomObjective;

        public override void Setup(bool _setActive = true)
        {
            grid = GameManager.I.MainGridCtrl; ;
            animator = GetComponentInChildren<Animator>();
            base.Setup(_setActive);
        }

        protected override void OnUpdate()
        {
            FollowPath();
        }

        public void FollowPath()
        {
            if (nodePath == null || nodePath.Count <= 0)
                return;
            if (Vector3.Distance(nodePath[0].spacePosition, transform.position) <= InteractionRadius)
                nodePath.Remove(nodePath[0]);
            if (nodePath.Count == 0)
                return;

            if (!pathTracking.IsActive())
            {
                pathTracking = transform.DOLookAt(nodePath[0].spacePosition, LookDuration).OnComplete(()=> 
                {
                    pathTracking = transform.DOMove(nodePath[0].spacePosition, MoveDuration);
                });
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
}