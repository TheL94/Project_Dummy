using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Generic;
using Framework.AI;

namespace DumbProject.GDR_System
{
    public class Enemy : MonoBehaviour, IInteractable, I_GDR_Interactable, IPreviewable
    {
        public GDR_Controller gdrController;
        public Cell RelativeCell { get; private set; }
        public EnemyData Data { get; private set; }

        float attackSpeed;

        AI_Controller AI_Ctrl; //da rivedere!

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as EnemyData;
            gdrController = gameObject.AddComponent<GDR_Controller>();
            gdrController.Init(GameManager.I.GDR_ElementDataMng.GetGDR_DataByID(Data.DataID));
            attackSpeed = gdrController.Data.Speed;
            IsInteractable = true;
        }

        public void SetRelativeCell(Cell _relativeCell)
        {
            RelativeCell = _relativeCell;
        }

        private void Update()
        {
            //ProximityCheck();
        }

        void ProximityCheck()
        {
            //TODO : Da rivedere
            if (RelativeCell == null || RelativeCell.RelativeRoom.StatusOfExploration != ExplorationStatus.InExploration)
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

        public bool Interact(AI_Controller _controller)
        {
            if (CoolDown())
            {
                AI_Ctrl = _controller;
                GDR_Interact(_controller.GetComponent<GDR_Controller>());
                attackSpeed = gdrController.Data.Speed;

                Debug.Log("Enemy Life " + gdrController.Data.Life);
                if (gdrController != null && (gdrController.Data.Life <= 0 || !IsInteractable))
                {
                    IsInteractable = false;
                    Destroy(gameObject);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region I_GDR_Interactable
        public GDR_Element_Generic_Data GDR_Data { get { return Data as GDR_Element_Generic_Data; } }

        public void GDR_Interact(GDR_Controller _GDR_Controller)
        {
            if (_GDR_Controller != null)
                _GDR_Controller.OnInteract(this, AI_Ctrl);

            AI_Ctrl = null;
        }
        #endregion

        bool CoolDown()
        {
            attackSpeed -= Time.deltaTime;
            if (attackSpeed <= 0)
                return true;
            else
                return false;
        }
    }
}