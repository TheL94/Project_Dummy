using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Items
{
    public interface IDroppable : IInteractable
    {
        ItemGenericData Data { get; set; }
        void Init(ItemGenericData _data);
    }
}