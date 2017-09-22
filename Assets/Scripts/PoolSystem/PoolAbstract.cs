using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework.PoolSystem
{
    public abstract class Pool<PoolType>
    {
        public List<PoolType> inactivePool = new List<PoolType>();
        public List<PoolType> activePool = new List<PoolType>();

        protected GameObject prefabReference;
        protected Transform parentObject;

        public virtual PoolType Get()
        {
            UpdatePools();

            if (inactivePool.Count > 0)
                return GetInactiveObject();
            else
            {
                PoolType item = InstantiateInstance();
                activePool.Add(item);
                return item;
            }
        }

        public virtual void UpdatePools()
        {
            for (int i = 0; i < activePool.Count; i++)
            {
                PoolType item = activePool[i];
                if (!IsObjectActive(item))
                {
                    activePool.Remove(item);
                    inactivePool.Add(item);
                    ResetPool(item);
                }
            }
        }

        public void ForcePoolsReset()
        {
            for (int i = 0; i < activePool.Count; i++)
            {
                PoolType item = activePool[i];
                ChangeObjectState(item, false);
                activePool.Remove(item);
                inactivePool.Add(item);
                ResetPool(item);
            }
        }

        protected void InitializeObjectPool(GameObject _prefabReference, Vector3 _containerPosition, Transform _containerParent, int _initialQuantity)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = _containerParent;
            obj.transform.position = _containerPosition;
            parentObject = obj.transform;
            parentObject.name = "Parent: " + _prefabReference.name;

            prefabReference = _prefabReference;
            for (int i = 0; i < _initialQuantity; i++)
            {
                PoolType item = InstantiateInstance();
                inactivePool.Add(item);
                ChangeObjectState(item, false);
            }
        }

        protected PoolType GetInactiveObject()
        {
            PoolType item = inactivePool[0];
            inactivePool.RemoveAt(0);
            activePool.Add(item);
            ChangeObjectState(item, true);
            return item;
        }

        PoolType InstantiateInstance()
        {
            GameObject instantiatedObject = GameObject.Instantiate(prefabReference);
            instantiatedObject.transform.parent = parentObject;
            PoolType typeInstance = GetPoolType(instantiatedObject);
            return typeInstance;
        }

        protected virtual void ResetPool(PoolType _item) { }

        protected abstract void ChangeObjectState(PoolType item, bool toState);
        protected abstract bool IsObjectActive(PoolType item);
        protected abstract PoolType GetPoolType(GameObject gameObject);
    }
}