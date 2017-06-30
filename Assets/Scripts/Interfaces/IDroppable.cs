using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject
{
    public interface IDroppable
    {
        Transform position { get; }

        void GetMyData(BaseData _data);
    }
}