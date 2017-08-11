using System.Linq;
using Framework.AI;
using Framework.Pathfinding;
using UnityEngine;

namespace DumbProject.Generic
{
    public class Dumby : AI_Controller, IPathfinder
    {
        #region IPathfinder
        public INetworkable Objective { get; set; }
        public INetworkable CurrentNetworkable { get; set; }

        public INetworkable[] Path { get; set; }
        #endregion
    }
}
