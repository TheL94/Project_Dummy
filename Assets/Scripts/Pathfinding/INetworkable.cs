using System.Collections.Generic;
using UnityEngine;

namespace Framework.Pathfinding {
    /// <summary>
    /// Inerface that make a class eligible for pathfinding
    /// </summary>
    public interface INetworkable
    {
        /// <summary>
        /// Actual position in space
        /// </summary>
        Vector3 spacePosition { get; set; }

        /// <summary>
        /// List of all the linked INetworkable
        /// Use AddLinks and RemoveLinks to work with
        /// </summary>
        List<INetworkable> Links { get; set; }

        /// <summary>
        /// Add _newLinks to Links
        /// </summary>
        /// <param name="_newLinks"> List of INetworkable to link</param>
        void AddLinks(List<INetworkable> _newLinks);

        /// <summary>
        /// Remove _linksToRemove from Links
        /// </summary>
        /// <param name="_linksToRemove"> INetworkable to remove from Links</param>
        void RemoveLinks(List<INetworkable> _linksToRemove);
    }

    public static class INetworkableExtetion
    {
        public static Vector3[] ToVector3Array(this List<INetworkable> INet)
        {
            Vector3[] arrayToReturn = new Vector3[INet.Count];

            for (int i = 0; i < INet.Count; i++)
            {
                arrayToReturn[i] = INet[i].spacePosition;
            }

            return arrayToReturn;
        }
    }
}
