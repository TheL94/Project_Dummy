using UnityEngine;
using System.Collections;

public class GameObjectPool : Pool<GameObject>
{

    public GameObjectPool(GameObject prefabReference)
    {
        InitializeObjectPool(prefabReference, 0);
    }

    public GameObjectPool(GameObject prefabReference, int initialQuantity)
    {
        InitializeObjectPool(prefabReference, initialQuantity);
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
