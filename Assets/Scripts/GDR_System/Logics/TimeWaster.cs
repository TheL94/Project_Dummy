using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AI;
using DumbProject.Generic;

namespace DumbProject.GDR_System
{
    public class TimeWaster : MonoBehaviour, IInteractable
    {
        public TimeWasterData Data { get; private set; }

        public bool IsInteractable { get; set; }
        public Transform Transf { get { return transform; } }

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as TimeWasterData;
        }

        public void Interact(AI_Controller _controller)
        {
            
        }
    }
}