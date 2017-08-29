using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Generic;
using DumbProject.Items;

namespace DumbProject.Rooms
{
    public class GDR_Controller : MonoBehaviour
    {
        public GDR_Data gdr_Data;

        public void Init(GDR_Data data)
        {
            data = gdr_Data;
            data.SetExperienceForNextLevel();
            data.ExperienceCounter = 0;
            data.PlayerLevel = 0;
            data.Speed = 1;
            data.Life = 1;
            data.Attack = 1;
            
        }
        private void Start()
        {
            Init(gdr_Data);
            
        }
        #region Debug
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))

                gdr_Data.ExperienceCounter ++;
            if (Input.GetKeyDown(KeyCode.DownArrow))

                if (gdr_Data.ExperienceCounter >0)
                {
                    gdr_Data.ExperienceCounter--; 
                }

        }
        #endregion


        
    }
}



