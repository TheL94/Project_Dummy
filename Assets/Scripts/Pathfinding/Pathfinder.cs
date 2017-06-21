using System.Linq;
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
        //Current INetworkable that enstablish position in network
        public INetworkable Actual
        {
            get { return _actual; }
            private set { _actual = value; }
        }

        Pathfinder() { }
        /// <summary>
        /// Constructor suggested to be used
        /// </summary>
        /// <param name="positionInNetwork"></param>
        public Pathfinder(INetworkable positionInNetwork)
        {
            Actual = positionInNetwork;
        }
        /// <summary>
        /// Find a path from _start to _target
        /// </summary>
        /// <param name="_target"> Point to head to</param>
        /// <param name="_start"> Starting position of the path to find</param>
        /// <returns> An ordered list of node to be used as position for the pathfinding</returns>
        public List<INetworkable> FindPath(INetworkable _target, INetworkable _start = null)
        {
            PathStep startingStep;
            //------Format to set Actual as defoult of this method's _start parameter
            if (_start == null)
                startingStep = new PathStep(Actual, Actual, _target);
            else
                startingStep = new PathStep(_start, _start, _target);

            List<PathStep> possiblePaths = new List<PathStep>() { startingStep };
            //-----------------------------------------------------------------------
            while (!CheckForNodeInPath(_target, possiblePaths))
            {
                possiblePaths.Add(FinNextValidStep(_target, possiblePaths));
            }

            List<INetworkable> foundPath = RetrackPath(possiblePaths, _target, _start == null ? Actual : _start);



            return foundPath;
        }


        /// <summary>
        /// Evaluate eligible nodes for pathfinding
        /// </summary>
        /// <param name="_target"></param>
        /// <param name="_givenPath"></param>
        /// <returns></returns>
        PathStep FinNextValidStep(INetworkable _target, List<PathStep> _givenPath)
        {
            PathStep outcome = new PathStep(_givenPath[0].node.Links[0], _givenPath[0].node, _target);
            float pathDistance = outcome.distance;

            for (int i = 0; i < _givenPath.Count; i++)
            {
                foreach (INetworkable closeNode in _givenPath[i].node.Links)
                {
                    PathStep tempOutCome = new PathStep(closeNode, _givenPath[0].node, _target);

                    if (CheckForNodeInPath(closeNode, _givenPath))
                        continue;

                    if (tempOutCome.distance < pathDistance)
                    {
                        outcome = tempOutCome;
                        pathDistance = outcome.distance;
                    }
                }
            }
            return outcome;
        }
        /// <summary>
        /// Evaluate the actual path starting from a list of already evaluated nodes
        /// </summary>
        /// <param name="_validPath"></param>
        /// <param name="_target"></param>
        /// <param name="_start"></param>
        /// <returns></returns>
        List<INetworkable> RetrackPath(List<PathStep> _validPath, INetworkable _target, INetworkable _start)
        {
            List<INetworkable> shortestPath = new List<INetworkable>();
            shortestPath.Add(_target);

            List<PathStep> _pathInEvaluation = _validPath.OrderBy(t => t.targetOffSet).ToList();
            PathStep nextToAdd = _validPath[0];
            while (!shortestPath.Contains(_start))
            {
                float distanceFromStart = _pathInEvaluation[0].originOffSet;
                for (int i = 0; i < _pathInEvaluation.Count; i++)
                {
                    if (!shortestPath[shortestPath.Count - 1].Links.Contains(_pathInEvaluation[i].node))
                        continue;

                    if (_pathInEvaluation[i].originOffSet < distanceFromStart)
                    {
                        nextToAdd = _pathInEvaluation[i];
                        distanceFromStart = _pathInEvaluation[i].originOffSet;
                    }
                }
                _pathInEvaluation.Remove(nextToAdd);
                shortestPath.Add(nextToAdd.node);
            }

            shortestPath.Reverse();
            return shortestPath;
        }
        /// <summary>
        /// Check if the _target node is already in the _givenPath list
        /// </summary>
        /// <param name="_node"></param>
        /// <param name="_givenPath"></param>
        /// <returns></returns>
        bool CheckForNodeInPath(INetworkable _node, List<PathStep> _givenPath)
        {
            foreach (PathStep step in _givenPath)
            {
                if (step.node.spacePosition == _node.spacePosition)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Convert a PathStep list to a INetworkable
        /// </summary>
        /// <param name="_PathSteps"></param>
        /// <returns></returns>
        List<INetworkable> PathStepToINetworkableList(List<PathStep> _PathSteps)
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

            public PathStep(INetworkable _node, INetworkable _startNode, INetworkable _targetNoide)
            {
                node = _node;
                startNode = _startNode;
                targetNode = _targetNoide;
            }
        }
    }
}
