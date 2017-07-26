using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Items
{
    public interface IDroppable : IInteractable
    {
        GenericDroppableData Data { get; set; }
        void Init(GenericDroppableData _data);
    }
}