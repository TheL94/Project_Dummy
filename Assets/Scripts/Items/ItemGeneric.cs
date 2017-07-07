using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class ItemGeneric : MonoBehaviour, IDroppable
    {
        SwordValues swordValues;
        PotionValues potionValues;
        ArmoryValues armoryValues;

        public Transform transF
        {
            get { return base.transform; }
        }
        DroppableBaseData _data;

        public DroppableBaseData Data
        {
            get { return _data; } 
            set { _data = value; }
        }


        public void InitIDroppable(DroppableBaseData _data)
        {
            Data = _data;
            switch (Data.Type)
            {
                case GenericType.Ememy:
                    break;
                case GenericType.Item:

                    switch ((Data as GenericData).SpecificType)
                    {
                        case ItemType.Sword:
                            swordValues = (Data as GenericData).SwordDataValues;
                            gameObject.AddComponent<Sword>().Init((_data as GenericData).SwordDataValues);
                            break;
                        case ItemType.Potion:
                            potionValues = (Data as GenericData).PotionDataValues;
                            gameObject.AddComponent<Potion>().Init((_data as GenericData).PotionDataValues);
                            break;
                        case ItemType.Armory:
                            armoryValues = (Data as GenericData).ArmoryDataValues;
                            gameObject.AddComponent<Armory>().Init((_data as GenericData).ArmoryDataValues);
                            break;
                        default:
                            break;
                    }
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
}