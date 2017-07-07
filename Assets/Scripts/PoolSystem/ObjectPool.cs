using UnityEngine;
using System.Collections;

namespace Framework.PoolSystem
{
    public class ObjectPool<PoolType> : Pool<PoolType> where PoolType : MonoBehaviour
    {
        public ObjectPool(GameObject _prefabReference, Vector3 _containerObj)
        {
            InitializeObjectPool(_prefabReference, _containerObj, 0);
        }

        public ObjectPool(GameObject _prefabReference, Vector3 _containerObj, int _initialQuantity)
        {
            InitializeObjectPool(_prefabReference, _containerObj, _initialQuantity);
        }

        protected override PoolType GetPoolType(GameObject gameObject)
        {
            PoolType type = gameObject.GetComponent<PoolType>();
            return type;
        }

        protected override void ResetPool(PoolType _item)
        {
            _item.transform.position = parentObject.transform.position;
            _item.transform.rotation = Quaternion.identity;
            _item.transform.parent = parentObject.transform;
        }

        protected override void ChangeObjectState(PoolType item, bool toState)
        {
            item.gameObject.SetActive(toState);
        }

        protected override bool IsObjectActive(PoolType item)
        {
            return item.gameObject.activeSelf;
        }
    }
}