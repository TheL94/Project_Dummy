using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.GDR;
using DumbProject.Generic;
using DumbProject.Rooms;

namespace DumbProject.GDR_System
{
    public class GDR_Element_Manager : MonoBehaviour
    {
        [HideInInspector]
        public List<GDR_Element_Generic_Data> Istances_GDR_Element_Data;
        [HideInInspector]
        public ProbabilityGroup<GDR_Element_Generic_Data> ProbGroup;

        #region Setup
        public void Init(List<GDR_Element_Generic_Data> _istances_GDR_Element_Data)
        {
            Istances_GDR_Element_Data = _istances_GDR_Element_Data;
            ProbGroup = new ProbabilityGroup<GDR_Element_Generic_Data>(new List<ProbabilityGroup<GDR_Element_Generic_Data>.Element<GDR_Element_Generic_Data>>());
            foreach (GDR_Element_Generic_Data item in Istances_GDR_Element_Data)
            {
                ProbGroup.Add(new ProbabilityGroup<GDR_Element_Generic_Data>.Element<GDR_Element_Generic_Data>(item, item.PercentageToSpawn));
            }
        } 
        #endregion

        #region API
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_room"></param>
        public void AddGDR_ElementInRoom(Room _room)
        {
            GDR_Element_Generic_Data element_Generic_Data = ProbGroup.PickRandom();
            if (element_Generic_Data != null)
            {
                IInteractable element = CreateInteractable(element_Generic_Data);
                _room.AddInteractable(element); 
            }
        }

        /// <summary>
        /// A seconda del tipo del data entra all'interno del sottotipo e viene aggiunto il component corrispondente
        /// chiamandone anche l'init dove vengono passati i dati relativi all'oggetto
        /// </summary>
        /// <param name="_data">Il data relativo all'oggetto che viene istanziato</param>
        IInteractable CreateInteractable(GDR_Element_Generic_Data _data)
        {
            GameObject graphicObj = Instantiate(_data.ElementPrefab);

            GameObject previewObj = Instantiate(_data.ElementPrefab, graphicObj.transform.position, graphicObj.transform.rotation, graphicObj.transform);
            previewObj.layer = 9; // Preview Layer
            foreach (Transform obj in previewObj.GetComponentsInChildren<Transform>())
            {
                obj.gameObject.layer = 9; // Preview Layer
            }       
            previewObj.transform.localScale = new Vector3(5f, 5f, 5f);

            IInteractable item = null;

            if (_data.GetType() == typeof(WeaponData))
            {
                item = graphicObj.AddComponent<Weapon>();
                (item as Weapon).Init(_data);
            }
            else if (_data.GetType() == typeof(PotionData))
            {
                item = graphicObj.AddComponent<Potion>();
                (item as Potion).Init(_data);
            }
            else if (_data.GetType() == typeof(ArmorData))
            {
                item = graphicObj.AddComponent<Armor>();
                (item as Armor).Init(_data);
            }
            else if (_data.GetType() == typeof(TimeWasterData))
            {
                item = graphicObj.AddComponent<TimeWaster>();
                (item as TimeWaster).Init(_data);
            }
            else if (_data.GetType() == typeof(TrapData))
            {
                item = graphicObj.AddComponent<Trap>();
                (item as Trap).Init(_data);
            }
            else if (_data.GetType() == typeof(EnemyData))
            {
                item = graphicObj.AddComponent<Enemy>();
                (item as Enemy).Init(_data);
            }
            return item;
        }
        #endregion
    }
}