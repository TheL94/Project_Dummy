using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms;
using Framework.AI;
using Framework.Pathfinding;

namespace DumbProject.Generic
{
    public class Dumby : AIController
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


        public override void Setup(bool _setActive = true)
        {
            grid = GameManager.I.MainGridCtrl; ;
            animator = GetComponentInChildren<Animator>();
            base.Setup(_setActive);
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