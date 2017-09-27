using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.GDR;

namespace DumbProject.Items
{
    public class TimeWaster : MonoBehaviour
    {
        public TimeWasterData Data;
        public Cell RelativeCell;
        public bool IsActive { get; private set; }

        public void Init(GenericDroppableData _values, Cell _relativeCell)
        {
            Data = _values as TimeWasterData;
            RelativeCell = _relativeCell;
        }
    }
}
