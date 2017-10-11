using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.GDR_System
{
    public interface IDroppable : IInteractable
    {
        GDR_Element_Generic_Data Data { get; set; }
        void Init(GDR_Element_Generic_Data _data);
    }
}