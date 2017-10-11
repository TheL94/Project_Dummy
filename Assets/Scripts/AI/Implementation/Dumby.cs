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
        }

        #region Animation
        Animator animator;
        private AnimationState _animState;
        public AnimationState AnimState
        {
            get { return _animState; }
            set
            {
                if (_animState == value)
                    return;

                _animState = value;
                if (animator != null)
                {
                    animator.SetInteger("AnimationState", (int)_animState);
                    //switch (_animState)
                    //{
                    //    case AnimationState.Idle:
                    //        animator.SetTrigger("Idle");
                    //        break;
                    //    case AnimationState.Run:
                    //        animator.SetTrigger("Run");
                    //        break;
                    //    case AnimationState.Fall:
                    //        animator.SetTrigger("Fall");
                    //        break;
                    //    case AnimationState.Headbutt:
                    //        animator.SetTrigger("Headbutt");
                    //        break;
                    //    case AnimationState.Dive:
                    //        animator.SetTrigger("Dive");
                    //        break;
                    //    case AnimationState.Death:
                    //        animator.SetTrigger("Death");
                    //        break;
                    //    default:
                    //        break;
                    //}
                }
            }
        }
        public enum AnimationState
        {
            Idle,
            Run,
            Fall,
            Headbutt,
            Dive,
            Death
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
