using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.GDR;

namespace DumbProject.GDR_System
{
    public interface I_GDR_Equippable
    {
        GameObject GameObj { get; }
        void Equip(GDR_Controller _controller);
    }
}