using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.GDR_System
{
    public interface I_GDR_Interactable
    {
        GDR_Element_Generic_Data GDR_Data { get; }
        void GDR_Interact(GDR_Controller _GDR_Controller);
    }
}