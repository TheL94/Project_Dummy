using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Generic;
using DumbProject.Items;
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
            Data.ai_Controller = GetComponent<AI_Controller>();//Da capire se la logica è corretta.
        }
        private void Start()
        {
            Init(Data);
            Data.ActualLife = 3;
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
        public void OnInteract(ItemGeneric _itemGeneric)
        {
            if (_itemGeneric == null)
            {
                Debug.LogWarning("item nullo");
                return;
            }
            if (_itemGeneric.GetType() == typeof(Potion))
            {
                Data.ActualLife += (_itemGeneric as Potion).PotionValues.HealtRestore;
                Debug.Log(Data.ActualLife);
            }
            else
            {
                Data.iC.OnPickUpItem(this, _itemGeneric);
                Debug.Log(_itemGeneric);
            }
        }
    }
}



