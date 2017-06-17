using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Pathfinding
{
    /// <summary>
    /// Componen used to navigate onto a INetworkable network
    /// </summary>
    public class Pathfinder
    {
        INetworkable _actual = null;
        public INetworkable Actual
        {
            get { return _actual; }
            private set { _actual = value; }
        }

        Pathfinder() { }
        public Pathfinder(INetworkable positionInNetwork)
        {
            Actual = positionInNetwork;
        }

        public List<INetworkable> FindPath(INetworkable _target, INetworkable _start = null)
        {
            List<INetworkable> foundPath = new List<INetworkable>();

            return foundPath;
        }

        List<PathStep> FindPath(INetworkable _target, List<PathStep> _givenPath)
        {
            return null;
        }

        List<INetworkable> PathStepToINetworkableList( List<PathStep> _PathSteps)
        {
            List<INetworkable> iNetPath = new List<INetworkable>();
            foreach (PathStep pS in _PathSteps)
            {
                iNetPath.Add(pS.node);
            }
            return iNetPath;
        }

        /// <summary>
        /// Class that identify each step of a Path
        /// </summary>
        class PathStep
        {
            public INetworkable node;
            public float distance
            {
                get { return originOffSet + targetOffSet; }
            }
            public float originOffSet;
            public float targetOffSet;

            PathStep() { }
            public PathStep(INetworkable _node, float _originOffSet, float _targetOffSet)
            {
                node = _node;
                originOffSet = _originOffSet;
                targetOffSet = _targetOffSet;
            }
        }
    }
}
