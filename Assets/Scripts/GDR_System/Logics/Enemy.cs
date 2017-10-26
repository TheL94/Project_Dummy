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
        public GDR_Controller enemy_GDR_Controller;
        public EnemyData Data { get; private set; }

        float attackSpeed;

        AI_Controller AI_Ctrl; //TODO : da rivedere!

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as EnemyData;
            enemy_GDR_Controller = gameObject.AddComponent<GDR_Controller>();
            enemy_GDR_Controller.Init(GameManager.I.GDR_ElementDataMng.GetGDR_DataByID(Data.DataID));
            attackSpeed = enemy_GDR_Controller.Data.Speed;
            IsInteractable = true;
        }

        public void Kill()
        {
            Destroy(gameObject);
        }

        #region IPreviewable
        public GameObject PreviewObj { get; set; }
        #endregion

        #region IInteractable
        public bool IsInteractable { get; set; }
        public Transform Transf { get { return transform; } }

        public bool Interact(AI_Controller _dumby_AI_Controller)
        {
            if (CoolDown())
            {
                AI_Ctrl = _dumby_AI_Controller;
                GDR_Interact(_dumby_AI_Controller.GetComponent<GDR_Controller>());
                attackSpeed = enemy_GDR_Controller.Data.Speed;
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