using UnityEngine;
using System.Collections;

public class ObjectPool<PoolType> : Pool<PoolType> where PoolType : MonoBehaviour
{

    public ObjectPool(GameObject prefabReference)
    {
        InitializeObjectPool(prefabReference, 0);
    }

    public ObjectPool(GameObject prefabReference, int initialQuantity)
    {
        InitializeObjectPool(prefabReference, initialQuantity);
    }

    protected override PoolType GetPoolType(GameObject gameObject)
    {
        PoolType type = gameObject.GetComponent<PoolType>();
        return type;
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
