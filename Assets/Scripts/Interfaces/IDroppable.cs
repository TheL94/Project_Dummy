using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Items
{
    public interface IDroppable : IInteractable
    {
        GenericItemData Data { get; set; }
        void Init(GenericItemData _data);
    }
}