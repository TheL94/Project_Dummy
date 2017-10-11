using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.GDR;
using Framework.AI;

namespace DumbProject.GDR_System
{
    public class Trap : MonoBehaviour, IInteractable, I_GDR_Interactable
    {
        public Cell RelativeCell;

        public TrapData Data { get; private set; }
        public bool IsInteractable { get; set; }
        public Transform Transf { get { return transform; } }

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as TrapData;
            IsInteractable = false;
        }

        private void Update()
        {
            ProximityCheck();
        }

        void ProximityCheck()
        {
            //TODO : Da rivedere
            if (RelativeCell.RelativeRoom.StatusOfExploration != ExplorationStatus.InExploration)
                return;

            if (Vector3.Distance(GameManager.I.Dumby.transform.position, transform.position) <= Data.ActivationRadius)
                Interact(GameManager.I.Dumby);
        }

        public void Interact(AI_Controller _controller)
        {
            GDR_Interact(_controller.GetComponent<GDR_Controller>());
        }

        public void GDR_Interact(GDR_Controller _GDR_Controller)
        {
            if (_GDR_Controller != null)
                _GDR_Controller.OnInteract(this);
        }
    }
}
