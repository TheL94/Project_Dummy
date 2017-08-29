using System.Linq;
using Framework.Pathfinding;
using System.Collections.Generic;
using DumbProject.Generic;
using UnityEngine;

namespace Framework.AI
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
            if ((_controller as IPathfinder).Path.Length > 0)
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
            if (_pathfinder.Path.Length <= 0)
            {
                Debug.LogWarning("Trying to follow a Path of 0 nodes");
                return;
            }

            //Actually move
            SmoothRotation(_pathfinder.SmoothedPath[0]);
            SmoothTranslation(_pathfinder.SmoothedPath[0]);

            //Each time a node is reached is set as current and removed from the current Path
            ChopPath(_pathfinder);
            if(_pathfinder.SmoothedPath.Length <= 0)
            {
                _pathfinder.CurrentNetworkable = _pathfinder.Path[_pathfinder.Path.Length - 1];
                _pathfinder.Path = new INetworkable[0];
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

        void ChopPath(IPathfinder _pathfinder)
        {
            //Check if distance has reached a minimum
            if (Vector3.Distance(_pathfinder.SmoothedPath[0], ctrlTransform.position) <= ProximityDetection)
            {
                _pathfinder.CurrentNetworkable = GameManager.I.MainGridCtrl.GetSpecificGridNode(_pathfinder.SmoothedPath[0]);
                _pathfinder.SmoothedPath = _pathfinder.SmoothedPath.Skip(1).ToArray();
            }
        }
    }
}
