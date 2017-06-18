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

            //------Format to set Actual as defoult of this method's _start parameter
            if (_start == null)
                foundPath.Add(Actual);
            else
                foundPath.Add(_start);

            List<PathStep> possiblePaths = new List<PathStep>() { new PathStep(foundPath[0], foundPath[0], _target) };
            //-----------------------------------------------------------------------
            FindPath(_target, possiblePaths);

            return foundPath;
        }

        List<PathStep> FindPath(INetworkable _target, List<PathStep> _givenPath)
        {
            List<PathStep> outcome = _givenPath;
            float pathDistance = -1;
            foreach (PathStep step in _givenPath)
            {
                foreach (INetworkable closeNode in step.node.Links)
                {
                    if (pathDistance == -1)
                    {
                        outcome.Add(new PathStep(closeNode, _givenPath[0].node, _target));
                        pathDistance = outcome[outcome.Count - 1].distance;
                    }
                    else if (outcome[outcome.Count - 1].distance < pathDistance)
                    {
                        outcome.Remove(outcome[outcome.Count - 1]);
                        outcome.Add(new PathStep(closeNode, _givenPath[0].node, _target));
                        pathDistance = outcome[outcome.Count - 1].distance;
                    }
                }
            }
            return outcome;
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
            public INetworkable targetNode { get; private set; }
            public INetworkable startNode { get; private set; }
            public float distance
            {
                get { return originOffSet + targetOffSet; }
            }
            public float originOffSet { get { return Vector3.Distance(node.spacePosition, startNode.spacePosition); } }
            public float targetOffSet { get { return Vector3.Distance(node.spacePosition, targetNode.spacePosition); } }

            PathStep() { }
            public PathStep(INetworkable _node, INetworkable _startNode, INetworkable _targetNoide)
            {
                node = _node;
                startNode = _startNode;
                targetNode = _targetNoide;
            }
        }
    }
}
