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

        Tweener pathTrack;
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
        AnimationStatus _animState = AnimationStatus.Idle;
        public AnimationStatus AnimState
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

        public void FollowPath(bool forceNew = false)
        {
            AnimState = AnimationStatus.Running;
            if (nodePath == null || nodePath.Count <= 0)
                return;

            Vector3[] wayPoints = nodePath.ToVector3Array();

            for (int i = 0; i < wayPoints.Length; i++)
            {
                Debug.Log(wayPoints[i]);
            }
            if(pathTrack == null)
                pathTrack = transform.DOPath(wayPoints, MoveDuration * nodePath.Count, PathType.Linear, PathMode.Full3D, 0, Color.magenta);
            else if(!pathTrack.IsPlaying() || forceNew)
                pathTrack = transform.DOPath(wayPoints, MoveDuration * nodePath.Count, PathType.Linear, PathMode.Full3D, 0, Color.magenta);
            pathTrack.OnComplete(() => { nodePath.Clear(); });
            pathTrack.SetSpeedBased();
        }
    }

    public enum AnimationStatus
    {
        Fallen = 0,
        Idle = 1,
        Running = 2,
        Fighting = 3
    }
}
