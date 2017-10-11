using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AI;
using DumbProject.Generic;
using DumbProject.GDR;

namespace DumbProject.GDR_System
{
    public class TimeWaster : MonoBehaviour, IInteractable, I_GDR_Interactable
    {
        public TimeWasterData Data { get; private set; }

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as TimeWasterData;
        }

        #region IInteractable
        public bool IsInteractable { get; set; }
        public Transform Transf { get { return transform; } }

        public void Interact(AI_Controller _controller)
        {
            IsInteractable = false;
            GDR_Interact(_controller.GetComponent<GDR_Controller>());
        }
        #endregion

        #region I_GDR_Interactable
        public GDR_Element_Generic_Data GDR_Data { get { return Data as GDR_Element_Generic_Data; } }

        public void GDR_Interact(GDR_Controller _GDR_Controller)
        {
            if (_GDR_Controller != null)
                _GDR_Controller.OnInteract(this);
        }
        #endregion
    }
}