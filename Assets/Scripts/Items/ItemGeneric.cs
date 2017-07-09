using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class ItemGeneric : MonoBehaviour, IDroppable
    {

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

        /// <summary>
        /// A seconda del tipo del data entra all'interno del sottotipo e viene aggiunto il component corrispondente
        /// chiamandone anche l'init dove vengono passati i valori relativi all'oggetto
        /// </summary>
        /// <param name="_data">Il data relativo all'oggetto che viene istanziato</param>
        public void InitIDroppable(DroppableBaseData _data)
        {
            Data = _data;
            switch (Data.Type)
            {
                /// Da completare con l'aggiunta del component corrispondente e la chiamata all'init passando i Values del data
                case GenericType.Ememy:

                    switch ((Data as GenericData).SpecificEnemyType)
                    {
                        case EnemyType.None:
                            break;
                        case EnemyType.Drago:
                            // AddComponente<Drago>().Init((Data as GenericData).DragonDataValues)
                            break;
                        case EnemyType.Goblin:
                            break;
                        default:
                            break;
                    }

                    break;

                case GenericType.Item:

                    switch ((Data as GenericData).SpecificItemType)
                    {
                        case ItemType.None:
                            break;
                        case ItemType.Sword:
                            gameObject.AddComponent<Sword>().Init((Data as GenericData).SwordDataValues);
                            break;
                        case ItemType.Potion:
                            gameObject.AddComponent<Potion>().Init((Data as GenericData).PotionDataValues);
                            break;
                        case ItemType.Armory:
                            gameObject.AddComponent<Armory>().Init((Data as GenericData).ArmoryDataValues);
                            break;
                        default:
                            break;
                    }
                    break;
                    
                    /// Da completare con l'aggiunta del component corrispondente e la chiamata all'init passando i Values del data
                case GenericType.Trap:
                    switch ((Data as GenericData).SpecificTrapType)
                    {
                        case TrapType.None:
                            break;
                        case TrapType.Tagliola:
                            break;
                        case TrapType.Catapulta:
                            break;
                        default:
                            break;
                    }
                    break;
                case GenericType.Gattini:
                    break;
                default:
                    break;
            }
            
        }
        
    }
}