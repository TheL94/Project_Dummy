﻿using DumbProject.Grid;
using UnityEngine;
using System.Collections.Generic;
using Framework.AI;
using System;
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
        public void Setup(GridController _grid)
        {
            base.Setup();
            grid = _grid;
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