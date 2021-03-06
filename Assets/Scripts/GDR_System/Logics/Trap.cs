﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Generic;
using Framework.AI;

namespace DumbProject.GDR_System
{
    public class Trap : MonoBehaviour, IInteractable, I_GDR_Interactable, IPreviewable
    {
        public Cell RelativeCell { get; private set; }
        public TrapData Data { get; private set; }

        bool isActive = true;

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as TrapData;
            IsInteractable = false;
        }

        public void SetRelativeCell(Cell _relativeCell)
        {
            RelativeCell = _relativeCell;
        }

        private void Update()
        {
            if(isActive)
                ProximityCheck();
        }

        void ProximityCheck()
        {
            //TODO : Da rivedere
            if (RelativeCell == null)
                return;

            if (Vector3.Distance(GameManager.I.Dumby.transform.position, transform.position) <= Data.ActivationRadius)
                Interact(GameManager.I.Dumby);
            else
            {
                if (!Data.ActiveOnce && !isActive)
                {
                    isActive = true;
                }
            }
        }

        #region IPreviewable
        public GameObject PreviewObj { get; set; }
        #endregion

        #region IInteractable
        public bool IsInteractable { get; set; }
        public Transform Transf { get { return transform; } }

        public bool Interact(AI_Controller _controller)
        {
            GDR_Interact(_controller.GetComponent<GDR_Controller>());
            isActive = false;           
            return true;
        }
        #endregion

        #region I_GDR_Interactable
        public GDR_Element_Generic_Data GDR_Data { get { return Data as GDR_Element_Generic_Data; } }

        public void GDR_Interact(GDR_Controller _GDR_Controller)
        {
            if (_GDR_Controller != null)
            {
                _GDR_Controller.OnInteract(this);
            }
        }
        #endregion
    }
}
