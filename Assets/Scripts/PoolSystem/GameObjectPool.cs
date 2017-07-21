using UnityEngine;
using System.Collections;

namespace Framework.PoolSystem
{
    public class GameObjectPool : Pool<GameObject>
    {
        public GameObjectPool(GameObject _prefabReference, Vector3 _containerObj, Transform _containerParent)
        {
            InitializeObjectPool(_prefabReference, _containerObj, _containerParent, 0);
        }

        public GameObjectPool(GameObject _prefabReference, Vector3 _containerObj, Transform _containerParent, int _initialQuantity)
        {
            InitializeObjectPool(_prefabReference, _containerObj, _containerParent, _initialQuantity);
        }

        protected override GameObject GetPoolType(GameObject gameObject)
        {
            return gameObject;
        }

        protected override void ResetPool(GameObject _item)
        {
            _item.transform.position = parentObject.transform.position;
            _item.transform.parent = parentObject.transform;
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