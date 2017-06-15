using System.Collections.Generic;

namespace Framework.Pathfinding {
    /// <summary>
    /// Inerface that make a class eligible for pathfinding
    /// </summary>
    public interface INetworkable
    {
        /// <summary>
        /// List of all the linked INetworkable
        /// Use AddLinks and RemoveLinks to work with
        /// </summary>
        List<INetworkable> Links { get; set; }

        /// <summary>
        /// Actual pathfinding algorithm
        /// </summary>
        /// <param name="_target">Objective to reach</param>
        /// <returns>The closest INetworkable to _target</returns>
        List<INetworkable> GetNextCloser(INetworkable _target);

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
}
