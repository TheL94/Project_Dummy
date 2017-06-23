using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject
{
    public class BaseData : ScriptableObject
    {
        public BaseType Type;
        public GameObject Prefab;
    }

    public enum BaseType
    {
        Item,
        Enemy,
        Interactable,
    }
}