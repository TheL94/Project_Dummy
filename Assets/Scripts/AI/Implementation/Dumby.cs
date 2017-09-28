using DumbProject.Rooms;
using Framework.AI;
using Framework.Pathfinding;
using UnityEngine;
using System.Collections.Generic;

namespace DumbProject.Generic
{
    public class Dumby : AI_Controller, IPathfinder
    {
        private Room _currentRoom;
        public Room CurrentRoom
        {
            get
            {
                if (_currentRoom == null)
                    _currentRoom = GameManager.I.MainGridCtrl.GetSpecificGridNode(transform.position).RelativeCell.RelativeRoom;
                return _currentRoom;
            }
            protected set { _currentRoom = value; }
        }

        protected override void OnInit()
        {
            animator = GetComponentInChildren<Animator>();

            AnimationState uno = AnimationState.Idle1;
            AnimationState due = AnimationState.Idle2;
            AnimationState tre = AnimationState.Headbutt;
            Debug.Log((int)uno + (int)due + (int)tre);
        }

        #region Animation
        Animator animator;
        private AnimationState _animState;
        public AnimationState AnimState
        {
            get { return _animState; }
            set
            {
                _animState = value;
                if (animator != null)
                    animator.SetInteger("AnimatorState", (int)_animState);
            }
        }
        public enum AnimationState
        {
            Idle1 = 1,
            Idle2,
            Headbutt
        }
        #endregion

        #region IPathfinder
        public INetworkable Objective { get; set; }
        private INetworkable _currentNetworkable;
        public INetworkable CurrentNetworkable
        {
            get
            {
                if (_currentNetworkable == null)
                    _currentNetworkable = GameManager.I.MainGridCtrl.GetSpecificNetNode(transform.position);
                if (_currentNetworkable == null)
                    _currentNetworkable = GameManager.I.MainGridCtrl.GetSpecificGridNode(transform.position);
                return _currentNetworkable;
            }
            set
            {
                if (GameManager.I.MainGridCtrl.GetSpecificNetNode(value.spacePosition) == null)
                {
                    Cell currentCell = GameManager.I.MainGridCtrl.GetSpecificGridNode(value.spacePosition).RelativeCell;
                    if(currentCell != null)
                        CurrentRoom = GameManager.I.MainGridCtrl.GetSpecificGridNode(value.spacePosition).RelativeCell.RelativeRoom;
                }
                _currentNetworkable = value;
            }
        }

        private Path _path = new Path();
        public Path Path { get { return _path; } set { _path = value; } }
        #endregion
    }
}
