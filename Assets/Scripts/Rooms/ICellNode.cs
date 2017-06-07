using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Rooms.Cells
{
    public interface ICellNode
    {
        Cell RelativeCell { get; set; }
    }
}
