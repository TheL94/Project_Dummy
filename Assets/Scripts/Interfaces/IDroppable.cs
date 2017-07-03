using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public interface IDroppable
    {
        Transform transF { get; }

        void GetMyData(BaseData _data);
    }
}