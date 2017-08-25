using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using Framework.PoolSystem;

namespace DumbProject.Generic
{
    public class PoolManager : MonoBehaviour
    {
        List<RoomGraphicComponent> roomGraphicComponents;
        List<PoolType> gameObjPoolList = new List<PoolType>();

        public void Init(List<RoomGraphicComponent> _roomGraphicComponents)
        {
            roomGraphicComponents = _roomGraphicComponents;
        }

        public void Setup()
        {
            SetupPool();
        }

        public GameObject GetGameObject(string _id)
        {
            return GetPool(_id).Get();
        }

        public void UpdatePools()
        {
            foreach (PoolType pool in gameObjPoolList)
            {
                pool.Pool.UpdatePools();
            }
        }

        public void ForceAllPoolsReset()
        {
            foreach (PoolType pool in gameObjPoolList)
            {
                pool.Pool.ForcePoolsReset();
            }
        }

        GameObjectPool GetPool(string _id)
        {
            foreach (PoolType pool in gameObjPoolList)
                if (pool.ID == _id)
                    return pool.Pool;
            return null;
        }

        void SetupPool()
        {
            GameObjectPool gameObjPool = null;
            Vector3 containerPosition = Camera.main.transform.forward * -10000;

            foreach (RoomGraphicComponent graphicComponent in roomGraphicComponents)
            {
                gameObjPool = new GameObjectPool(graphicComponent.ObjPrefab, containerPosition, transform, graphicComponent.NumberOfObj);
                gameObjPoolList.Add(new PoolType(gameObjPool, graphicComponent.ID));
            }
        }
    }

    public class PoolType
    {
        public GameObjectPool Pool;
        public string ID;

        public PoolType(GameObjectPool _pool, string _id)
        {
            Pool = _pool;
            ID = _id;
        }
    }
}