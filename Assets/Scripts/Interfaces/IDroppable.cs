using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public interface IDroppable
    {
        Transform transF { get; }
        ItemType Type { get; }
        void GetMyData(DroppableBaseData _data);
    }
}