using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Rooms
{
    public interface ICellNode
    {
        Cell RelativeCell { get; set; }
    }
}
