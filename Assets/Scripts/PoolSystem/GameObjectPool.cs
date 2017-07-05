using UnityEngine;
using System.Collections;

namespace Framework.PoolSystem
{
    public class GameObjectPool : Pool<GameObject>
    {
        public GameObjectPool(GameObject _prefabReference, Vector3 _containerObj)
        {
            InitializeObjectPool(_prefabReference, _containerObj, 0);
        }

        public GameObjectPool(GameObject _prefabReference, Vector3 _containerObj, int _initialQuantity)
        {
            InitializeObjectPool(_prefabReference, _containerObj, _initialQuantity);
        }

        protected override GameObject GetPoolType(GameObject gameObject)
        {
            return gameObject;
        }

        protected override void ChangeObjectState(GameObject item, bool toState)
        {
            item.SetActive(toState);
        }

        protected override bool IsObjectActive(GameObject item)
        {
            return item.activeSelf;
        }
    }
}