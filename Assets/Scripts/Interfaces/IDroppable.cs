using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public interface IDroppable
    {
        DroppableBaseData Data { get; set; }
        Transform transF { get; }
        void InitIDroppable(DroppableBaseData _data);
    }
}