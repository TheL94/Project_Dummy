using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Pathfinding
{
    public class Path
    {
        protected INetworkable[] originalPath = new INetworkable[0];
        protected Vector3[] smoothedPath;
        protected int smoothedPathIndex;

        protected BezierPath BPath = new BezierPath();
        protected void OnPathSet()
        {
            List<Vector3> pathPositions = new List<Vector3>();

            foreach (INetworkable node in originalPath)
            {
                pathPositions.Add(node.spacePosition);
            }

            BPath.Interpolate(pathPositions, 100);
            smoothedPath = pathPositions.ToArray();
        }

        #region  API
        public void SetOriginalPath(INetworkable[] _nodes)
        {
            originalPath = _nodes;
            OnPathSet();
        }

        public INetworkable[] GetOriginalPath()
        {
            return originalPath;
        }

        public Vector3[] GetSmoothedPath()
        {
            return smoothedPath;
        }

        public Vector3[] GetRemainingSmoothedPath()
        {
            List<Vector3> choppedPath = new List<Vector3>();
            if (smoothedPath == null)
                return null;

            choppedPath.AddRange(smoothedPath);

            choppedPath.RemoveRange(0, smoothedPathIndex);

            return choppedPath.ToArray();
        }

        public Vector3 GetNextPosition()
        {
            return smoothedPath[smoothedPathIndex];
        }

        public bool ProceedOneForwardOnSmoothedPath()
        {
            smoothedPathIndex++;
            if(smoothedPathIndex < smoothedPath.Length)
            {
                return true;
            }
            else
            {
                smoothedPathIndex--;
                return false;
            }
        }

        public INetworkable GetCloserINetworkableOfPath()
        {
            INetworkable outcome = originalPath[originalPath.Length -1];
            float distance = Vector3.Distance(originalPath[originalPath.Length-1].spacePosition, smoothedPath[smoothedPathIndex]);
            for (int i = 0; i < originalPath.Length -1; i++)
            {
                float tempDis = Vector3.Distance(originalPath[i].spacePosition, smoothedPath[smoothedPathIndex]);
                if(tempDis < distance)
                {
                    distance = tempDis;
                    outcome = originalPath[i];
                }
            }

            return outcome;
        }
        #endregion
    }
}