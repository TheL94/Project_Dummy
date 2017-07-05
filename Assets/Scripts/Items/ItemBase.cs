using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class ItemBase : MonoBehaviour
    {
        public ItemType type;


        void SetSpecificType(GenericType _type)
        {
            switch (_type)
            {
                case GenericType.Ememy:
                    break;
                case GenericType.Item:
                    break;
                case GenericType.Trap:
                    break;
                case GenericType.Gattini:
                    break;
                default:
                    break;
            }
        }

    }

    public enum ItemType
    {
        Sward,
        Potion,
        Armory,
    }

    public enum EnemyType
    {
        Drago,
        Goblin,
    }

    public enum TrapType
    {
        Morsa,
        Catapulta,
    }

    public enum GattiniType
    {
        Siamesi,
        Persiani,
    }

}