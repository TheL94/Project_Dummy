using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Generic;
using DumbProject.GDR_System;
using DumbProject.Rooms;
using Framework.AI;

namespace DumbProject.GDR
{
    public class GDR_Controller : MonoBehaviour
    {
        public GDR_Data Data;

        public void Init(GDR_Data data)
        { 
            data = Data;
           // Data.ai_Controller = GetComponent<AI_Controller>();
        }

        private void Start()
        {
            Init(Data);
            Data.iC.Init();
        }

        /// <summary>
        /// Create and Instantiate a new GDR Data
        /// </summary>
        public GDR_Controller CreateGDR(GDR_Data _gdr_Data)
        {
            if (_gdr_Data)
            {
                GDR_Data NewIstanceGDRData;
                NewIstanceGDRData = Instantiate(_gdr_Data);
                GDR_Controller NewIstanceGDR = Instantiate(NewIstanceGDRData.GDRPrefab);
                NewIstanceGDR.Init(NewIstanceGDRData);
                return NewIstanceGDR;
            }
            return null;
        }

        /// <summary>
        /// Chiamata quando viene raccolto un oggetto da una cella.
        /// </summary>
        public void OnInteract(I_GDR_Interactable _GDR_Interactable)
        {
            if (_GDR_Interactable == null)
            {
                Debug.LogWarning("item nullo");
                return;
            }
            if (_GDR_Interactable.GetType() == typeof(Potion))
            {
                Data.GetCure((_GDR_Interactable as Potion).Data.HealtRestore);
                Debug.Log(Data.Life);
            }
            else if (_GDR_Interactable.GetType() == typeof(Trap))
            {
                Trap trap = (_GDR_Interactable as Trap);
                if (Data.GetDamage(trap.Data.Damage))
                {
                    Data.GetExperience(ExperienceType.Trap, trap.Data);
                }
            }
            else if (_GDR_Interactable.GetType() == typeof(TimeWaster))
            {
                Data.GetExperience(ExperienceType.Trap, (_GDR_Interactable as TimeWaster).Data);
            }
            else
            {
                Data.iC.OnPickUpItem(this, _GDR_Interactable);
                Debug.Log(_GDR_Interactable);
                if(_GDR_Interactable.GetType() == typeof(Armor))
                {
                    Data.MaxArmor = (_GDR_Interactable as Armor).Data.Protection;
                }
            }
        }
    }
}



