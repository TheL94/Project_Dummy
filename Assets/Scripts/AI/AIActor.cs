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

        protected INetworkable _currentNode;
        /// <summary>
        /// Actual position onto the INetworkable grid. 
        /// Can't be Set. Any try will change nothing.
        /// </summary>
        public override INetworkable CurrentNode
        {
            get
            {
                if (_currentNode == null)
                    _currentNode = Grid.GetSpecificGridNode(transform.position);
                return _currentNode;
            }
            set
            {
                _currentNode = value;
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

        /// <summary>
        /// Va muovere Dumby di un passo alla volta di nodo in nodo
        /// </summary>
        public void MoveToNextPathNode()
        {
            AnimState = AnimationStatus.Running;
            if (NodePath == null || NodePath.Count <= 0)
            {
                AnimState = AnimationStatus.Idle;
                return;
            }

            if (pathTrack == null || !pathTrack.IsActive() || pathTrack.IsComplete())
            {
                pathTrack = transform.DOLookAt(NodePath[0].spacePosition, LookDuration).OnComplete(() =>
                {
                    pathTrack = transform.DOMove(NodePath[0].spacePosition, MoveDuration).OnComplete(() => 
                    {
                        CurrentNode = NodePath[0];
                        NodePath.Remove(NodePath[0]);
                        pathTrack.Kill();
                    });
                });
            }
        }

        public void SetPath(List<INetworkable> _networkables)
        {
            if (pathTrack != null)
                pathTrack.Complete();
            NodePath = _networkables;
            BlockPathWithObstacles();
        }

        public List<IInteractable> GetCurrentCellInteractables()
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
