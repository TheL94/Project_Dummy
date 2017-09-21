using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Pathfinding
{
    public interface IPathfinder
    {
        INetworkable Objective { get; set; }
        INetworkable CurrentNetworkable { get; set; }

        Path Path { get; set; }
    }
}
