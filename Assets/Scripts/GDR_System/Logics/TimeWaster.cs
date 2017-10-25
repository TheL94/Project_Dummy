using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AI;
using DumbProject.Generic;

namespace DumbProject.GDR_System
{
    public class TimeWaster : MonoBehaviour, IInteractable, I_GDR_Interactable, IPreviewable
    {
        public TimeWasterData Data { get; private set; }
        float timeWasted;

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as TimeWasterData;
            timeWasted = Data.TimeToSpend;
            IsInteractable = true;
        }

        #region IPreviewable
        public GameObject PreviewObj { get; set; }
        #endregion

        #region IInteractable
        public bool IsInteractable { get; set; }
        public Transform Transf { get { return transform; } }

        public bool Interact(AI_Controller _controller)
        {
            if (CoolDown())
            {
                IsInteractable = false;
                timeWasted = Data.TimeToSpend;
                GDR_Interact(_controller.GetComponent<GDR_Controller>());
                return true;
            }

            return false;
        }

        bool CoolDown()
        {
            timeWasted -= Time.deltaTime;
            if (timeWasted <= 0)
                return true;
            else
                return false;
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