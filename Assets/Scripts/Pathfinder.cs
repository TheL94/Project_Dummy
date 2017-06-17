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

        public List<INetworkable> FindPath(INetworkable _target, List<INetworkable> _startingPosition = null)
        {
            if (_startingPosition == null)                                                                      //Format to make it easier to use
                _startingPosition = new List<INetworkable>() { Actual };

            List<INetworkable> path = new List<INetworkable>();
            if (_startingPosition.Contains(_target))                                                            //Return a empty list if the target is reached
                return path;

            float distance = Vector3.Distance(Actual.spacePosition, _target.spacePosition);

            foreach (INetworkable position in _startingPosition)                                                //Reset distance with the shortest possible, due to possible iteration of this method
            {
                if (Vector3.Distance(position.spacePosition, _target.spacePosition) < distance)
                    distance = Vector3.Distance(position.spacePosition, _target.spacePosition);
            }

            foreach (INetworkable closePosition in _startingPosition)                                           //Evaluate the next closer INetworkable
            {
                foreach (INetworkable link in closePosition.Links)
                {
                    float newDistance = Vector3.Distance(link.spacePosition, _target.spacePosition);
                    if (newDistance <= distance)
                    {
                        if (newDistance < distance)
                        {
                            path.Clear();
                            distance = newDistance;
                        }
                        path.Add(link);
                    }
                }
            }

            if (path.Count == 0)
                return null;

            return path;
        }
    }
}
