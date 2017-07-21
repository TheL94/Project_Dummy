using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using Framework.PoolSystem;

namespace DumbProject.Generic
{
    public class PoolManager : MonoBehaviour
    {
        public RoomData RoomTypesData;
        RoomData roomTypesInstances;
        List<PoolType> gameObjPoolList = new List<PoolType>();

        public void Setup()
        {
            roomTypesInstances = Instantiate(RoomTypesData);
            SetupPool();
        }

        public GameObject GetGameObject(ObjType _type)
        {
            return GetPool(_type).Get();
        }

        public void UpdatePools()
        {
            foreach (PoolType pool in gameObjPoolList)
            {
                pool.Pool.UpdatePools();
            }
        }

        GameObjectPool GetPool(ObjType _type)
        {
            foreach (PoolType pool in gameObjPoolList)
                if (pool.Type == _type)
                    return pool.Pool;
            return null;
        }

        void SetupPool()
        {
            GameObjectPool gameObjPool = null;
            Vector3 containerPosition = Camera.main.transform.forward * -10000;

            gameObjPool = new GameObjectPool(roomTypesInstances.RoomElements.Arch.ObjPrefab, containerPosition, transform, roomTypesInstances.RoomElements.Arch.NumberOfObj);
            gameObjPoolList.Add(new PoolType(gameObjPool, ObjType.Arch));

            gameObjPool = new GameObjectPool(roomTypesInstances.RoomElements.Floor.ObjPrefab, containerPosition, transform, roomTypesInstances.RoomElements.Floor.NumberOfObj);
            gameObjPoolList.Add(new PoolType(gameObjPool, ObjType.Floor));

            gameObjPool = new GameObjectPool(roomTypesInstances.RoomElements.Pillar.ObjPrefab, containerPosition, transform, roomTypesInstances.RoomElements.Pillar.NumberOfObj);
            gameObjPoolList.Add(new PoolType(gameObjPool, ObjType.Pillar));

            gameObjPool = new GameObjectPool(roomTypesInstances.RoomElements.Wall.ObjPrefab, containerPosition, transform, roomTypesInstances.RoomElements.Wall.NumberOfObj);
            gameObjPoolList.Add(new PoolType(gameObjPool, ObjType.Wall));
        }
    }

    public class PoolType
    {
        public GameObjectPool Pool;
        public ObjType Type;

        public PoolType(GameObjectPool _pool, ObjType _type)
        {
            Pool = _pool;
            Type = _type;
        }
    }

    public enum ObjType
    {
        Arch,
        Floor,
        Pillar,
        Wall
    }
}