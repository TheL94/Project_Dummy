using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Generic {
    public class Timer : MonoBehaviour
    {

        public float TimeToWait;
        public GameObject Owner;
        public bool IsOver;
        float time = 0;

        public void Setup(GameObject _owner, float _timeToWait)
        {
            Owner = _owner;
            TimeToWait = _timeToWait;
            IsOver = false;
        }

        private void Update()
        {
            if (time >= TimeToWait)
                IsOver = true;

            if (!IsOver)
                time += Time.deltaTime;
        }
    }
}
