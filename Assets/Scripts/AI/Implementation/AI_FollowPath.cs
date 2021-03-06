﻿using Framework.Pathfinding;
using Framework.AI;
using UnityEngine;

namespace DumbProject.Generic
{
    [CreateAssetMenu(menuName = "AI/NewAction/FollowPath")]
    public class AI_FollowPath : AI_Action
    {
        public float RunSpeed;
        public float RotationSpeed;
        public float ProximityDetection = 0.01f;
        Transform ctrlTransform;

        protected override bool Act(AI_Controller _controller)
        {
            ctrlTransform = _controller.transform;

            FollowPath(_controller as IPathfinder);
            if ((_controller as IPathfinder).Path.GetOriginalPath().Length > 0)
                return false;
            else
                return true;
        }


        /// <summary>
        /// Head toward the next node of the CurrentPath
        /// </summary>
        public void FollowPath(IPathfinder _pathfinder)
        {
            //Prevent movement if there is no path
            if (_pathfinder.Path.GetOriginalPath().Length <= 0)
            {
                Debug.LogWarning("Trying to follow a Path of 0 nodes");
                return;
            }

            //Actually move
            SmoothRotation(_pathfinder.Path.GetNextPosition());
            SmoothTranslation(_pathfinder.Path.GetNextPosition());

            //Each time a node is reached is set as current and removed from the current Path
            ProceedOnPath(_pathfinder);
            if(_pathfinder.Path.GetNextPosition() == _pathfinder.CurrentNetworkable.spacePosition)
            {
                _pathfinder.Path = new Path();
            }
        }

        void SmoothRotation(Vector3 objective)
        {
            Quaternion rotationToApply = Quaternion.LookRotation(objective - ctrlTransform.position);
            rotationToApply = Quaternion.Slerp(ctrlTransform.rotation, rotationToApply, RotationSpeed * Time.deltaTime);

            ctrlTransform.rotation = rotationToApply;
        }

        void SmoothTranslation(Vector3 objective)
        {
            //Vector3 positionToApply = Vector3.Lerp(ctrlTransform.position, objective, RunSpeed * Time.deltaTime);
            Vector3 positionToApply = ctrlTransform.position + (objective - ctrlTransform.position).normalized * Time.deltaTime * RunSpeed;
            ctrlTransform.position = positionToApply;
        }

        void ProceedOnPath(IPathfinder _pathfinder)
        {
            //Check if distance has reached a minimum
            if (Vector3.Distance(_pathfinder.Path.GetNextPosition(), ctrlTransform.position) <= ProximityDetection)
            {
                _pathfinder.CurrentNetworkable = _pathfinder.Path.GetCloserINetworkableOfPath();
                _pathfinder.Path.ProceedOneForwardOnSmoothedPath();
            }
        }
    }
}
