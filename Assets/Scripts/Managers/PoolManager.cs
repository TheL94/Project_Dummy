using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Generic
{
    public class PoolManager : MonoBehaviour
    {
        public GameObject PrefabToPool;
        public int NumeberOfObj;
        Queue<GameObject> GameObjQueue = new Queue<GameObject>();

        public void Setup()
        {
            InstantiatePoolObjs(NumeberOfObj);
        }

        public GameObject GetPoolObj()
        {
            if (GameObjQueue.Count > 0)
            {
                GameObject tempObj = GameObjQueue.Dequeue();
                tempObj.transform.parent = null;
                return tempObj;
            }
            else
            {
                return Instantiate(PrefabToPool);
            }
        }

        public void ReturnPoolObj(GameObject _objToReturn)
        {
            if (_objToReturn != null)
            {
                _objToReturn.transform.position = transform.position;
                _objToReturn.transform.rotation = Quaternion.identity;
                _objToReturn.transform.parent = transform;
                GameObjQueue.Enqueue(_objToReturn);
            }
        }

        void InstantiatePoolObjs(int _numberOfObjs)
        {
            for (int i = 0; i < _numberOfObjs; i++)
            {
                GameObjQueue.Enqueue(Instantiate(PrefabToPool, transform.position, Quaternion.identity, transform));
            }
        }
    }
}