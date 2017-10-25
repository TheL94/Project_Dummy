using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.GDR;
using Framework.AI;

namespace DumbProject.GDR_System
{
    public class Enemy : MonoBehaviour, IInteractable, I_GDR_Interactable, IPreviewable
    {
        public GDR_Controller gdrController;
        public Cell RelativeCell { get; private set; }
        public EnemyData Data { get; private set; }

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as EnemyData;
            IsInteractable = true;
        }

        public void SetRelativeCell(Cell _relativeCell)
        {
            RelativeCell = _relativeCell;
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

        #region IPreviewable
        public GameObject PreviewObj { get; set; }
        #endregion

        #region IInteractable
        public bool IsInteractable { get; set; }
        public Transform Transf { get { return transform; } }

        public void Interact(AI_Controller _controller)
        {
            GDR_Interact(_controller.GetComponent<GDR_Controller>());
        }
        #endregion

        #region I_GDR_Interactable
        public GDR_Element_Generic_Data GDR_Data { get { return Data as GDR_Element_Generic_Data; } }

        public void GDR_Interact(GDR_Controller _GDR_Controller)
        {
            if (_GDR_Controller != null)
                _GDR_Controller.OnInteract(this);

            if (gdrController != null && (gdrController.Data.Life <= 0 || !IsInteractable))
            {
                IsInteractable = false;
            }
        }
        #endregion
    }
}