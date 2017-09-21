using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class Floor : CellElement
    {
        public override void DestroyObject(bool _destroyComponentOnly = false)
        {
            RelativeCell.Floor = null;
            base.DestroyObject(_destroyComponentOnly);
        }
    }
}

