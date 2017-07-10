using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public interface IDroppable : IInteractable
    {
        GenericItemData Data { get; set; }
        void Init(GenericItemData _data);
    }
}