using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Framework.AI
{
    public class GameManagerTest : MonoBehaviour
    {
        public Timer timer;
        public static GameManagerTest I;
        void Awake()
        {
            //Singleton paradigm
            if (I == null)
                I = this;
            else
                DestroyImmediate(gameObject);
        }
    }
}