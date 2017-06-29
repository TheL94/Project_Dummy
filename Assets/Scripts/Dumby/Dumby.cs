using DumbProject.Grid;
using Framework.Pathfinding;
using UnityEngine;
using System.Collections.Generic;
using Framework.AI;
using System;

namespace DumbProject.Generic
{
    [RequireComponent(typeof(AIController))]
    public class Dumby : MonoBehaviour, IAIControllable
    {
        Animator animator;
        AnimationState _animState = AnimationState.Idle;
        AnimationState animState
        {
            get { return _animState; }
            set
            {
                _animState = value;
                animator.SetInteger("AnimationState", (int)animState);
            }
        }

        GridController grid;
        public GridNode currentNode { get { return grid.GetSpecificGridNode(transform.position); } }

        public Vector3 position
        {
            get
            {
                return transform.position;
            }
        }

        AIController aiController;
        public Pathfinder pathFinder;
        public List<INetworkable> nodePath = new List<INetworkable>();

        

        public void Setup(GridController _grid)
        {
            grid = _grid;
            pathFinder = new Pathfinder(currentNode);
            aiController = GetComponent<AIController>();
            aiController.Setup(this);
            animator = GetComponentInChildren<Animator>();
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