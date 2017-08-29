using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    public class Timer : MonoBehaviour
    {

        public float TimeToWait;
        public float time = 0;
        public bool IsOver = false;
        public bool Active = false;

        void Update()
        {
            if (Active)
            {
                time += Time.deltaTime;

                if (time >= TimeToWait)
                {
                    IsOver = true;
                    time = 0;
                }
            }
        }
    }
}
