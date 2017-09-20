using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Generic;
using DumbProject.Items;

namespace DumbProject.GDR
{
    public class GDR_Controller : MonoBehaviour
    {
        public GDR_Data Data;

        public void Init(GDR_Data data)
        {
            data = Data;


        }
        private void Start()
        {
            Init(Data);            
        }
        #region Debug
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))

                Data.ExperienceCounter ++;
            if (Input.GetKeyDown(KeyCode.DownArrow))

                if (Data.ExperienceCounter >0)
                {
                    Data.ExperienceCounter--; 
                }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreateGDR(Data);
            }
        }
        #endregion

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

      
    }
}



