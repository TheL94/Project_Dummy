using Framework.AI;
using Framework.Pathfinding;
using DumbProject.Grid;
using DumbProject.Rooms;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace DumbProject.Generic
{
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
                return Grid.GetSpecificGridNode(transform.position);
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

        public GridController Grid { get; protected set; }
        public Cell CurrentCell { get { return Grid.GetSpecificGridNode(transform.position).RelativeCell; } }
        public Room CurrentRoom { get { return CurrentCell.RelativeRoom; } }

        public INetworkable INetworkableObjective { get; set; }
        List<INetworkable> _nodePath = new List<INetworkable>();
        public new List<INetworkable> NodePath
        {
            get { return _nodePath; }
            set {
                pathTrack.Kill();
                _nodePath = value; }
        }

        /// <summary>
        /// Va muovere Dumby di un passo alla volta di nodo in nodo
        /// </summary>
        /// <param name="forceNew"></param>
        public void MoveToNextPathNode(bool forceNew = false)
        {
            BlockPathWithObstacles();
            AnimState = AnimationStatus.Running;
            if (NodePath == null || NodePath.Count <= 0)
            {
                AnimState = AnimationStatus.Idle;
                return;
            }

            if (pathTrack == null || !pathTrack.IsPlaying())
            {
                Vector3[] waypoints = NodePath.ToVector3Array();
                pathTrack = transform.DOPath(waypoints, MoveDuration, PathType.CatmullRom, PathMode.Full3D, 5, Color.magenta);
                pathTrack.SetSpeedBased();
                pathTrack.OnComplete(() => {
                    NodePath.Clear();
                    pathTrack.Kill();
                });
            }


            //Vector3 wayPoint = NodePath[0].spacePosition;
            //if (pathTrack == null)
            //{
            //    pathTrack = transform.DOLookAt(wayPoint, LookDuration).OnComplete(() =>
            //    {
            //        //Debug.Log("Finished looking at: " + wayPoint);
            //        pathTrack = transform.DOMove(wayPoint, MoveDuration).OnComplete(() =>
            //        {
            //            //Debug.Log("Finished moving to: " + wayPoint);
            //            INetworkable nodeToRemove = NodePath[0];
            //            NodePath.Remove(nodeToRemove);
            //            pathTrack = null;
            //        });
            //    });
            //}
        }

        public List<IInteractable> GetCurrentCellInteractables ()
        {
            List<IInteractable> interactionToReturn = new List<IInteractable>();

            interactionToReturn.Add(CurrentCell.ActualInteractable);
            foreach (INetworkable node in CurrentCell.RelativeNode.Links)
            {
                if (node.GetType() == typeof(Edge))
                    interactionToReturn.Add(node as IInteractable);
            }

            interactionToReturn = interactionToReturn.Where(i => i != null).ToList();
            interactionToReturn = interactionToReturn.OrderBy(i => Vector3.Distance(i.Transf.position, transform.position)).ToList();
            
            return interactionToReturn;
        }

        public virtual void BlockPathWithObstacles() { }
    }

    public enum AnimationStatus
    {
        Fallen = 0,
        Idle = 1,
        Running = 2,
        Fighting = 3
    }
}
