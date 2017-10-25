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
        public ProbabilityGroup<GDR_Element_Generic_Data> IInteractable_ProbGroup;

        [HideInInspector]
        public ProbabilityGroup<GDR_Element_Generic_Data> I_GDR_Equippable_ProbGroup;

        #region API
        public void Init(List<GDR_Element_Generic_Data> _istances_GDR_Element_Data)
        {
            Istances_GDR_Element_Data = _istances_GDR_Element_Data;

            IInteractable_ProbGroup = new ProbabilityGroup<GDR_Element_Generic_Data>(new List<ProbabilityGroup<GDR_Element_Generic_Data>.Element<GDR_Element_Generic_Data>>());
            I_GDR_Equippable_ProbGroup = new ProbabilityGroup<GDR_Element_Generic_Data>(new List<ProbabilityGroup<GDR_Element_Generic_Data>.Element<GDR_Element_Generic_Data>>());
            foreach (GDR_Element_Generic_Data GDR_Element in Istances_GDR_Element_Data)
            {
                if ((GDR_Element.GetType() == typeof(ArmorData)) || (GDR_Element.GetType() == typeof(WeaponData)) || (GDR_Element.GetType() == typeof(PotionData)))
                {
                    I_GDR_Equippable_ProbGroup.Add(new ProbabilityGroup<GDR_Element_Generic_Data>.Element<GDR_Element_Generic_Data>(GDR_Element, GDR_Element.PercentageToSpawn));
                }
                else
                {
                    IInteractable_ProbGroup.Add(new ProbabilityGroup<GDR_Element_Generic_Data>.Element<GDR_Element_Generic_Data>(GDR_Element, GDR_Element.PercentageToSpawn));
                }
            }
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_room"></param>
        public void AddGDR_ElementInRoom(Room _room)
        {
            GDR_Element_Generic_Data element_Generic_Data = IInteractable_ProbGroup.PickRandom();
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
            IInteractable interactable = null;

            if (_data.GetType() == typeof(ChestData))
            {
                interactable = graphicObj.AddComponent<Chest>();
                (interactable as Chest).Init(_data);
                (interactable as I_GDR_EquippableHolder).Equippable = CreateEquippable((interactable as Chest).transform, I_GDR_Equippable_ProbGroup.PickRandom());
            }
            else if (_data.GetType() == typeof(TimeWasterData))
            {
                CreatePreviewObjectFromData(_data, new Vector3(3f, 3f, 3f), graphicObj.transform);
                interactable = graphicObj.AddComponent<TimeWaster>();
                (interactable as TimeWaster).Init(_data);
            }
            else if (_data.GetType() == typeof(TrapData))
            {
                CreatePreviewObjectFromData(_data, new Vector3(3f, 3f, 3f), graphicObj.transform);
                interactable = graphicObj.AddComponent<Trap>();
                (interactable as Trap).Init(_data);
            }
            else if (_data.GetType() == typeof(EnemyData))
            {
                CreatePreviewObjectFromData(_data, new Vector3(3f, 3f, 3f), graphicObj.transform);
                interactable = graphicObj.AddComponent<Enemy>();
                (interactable as Enemy).Init(_data);
            }
            return interactable;
        }

        I_GDR_Equippable CreateEquippable(Transform _parent, GDR_Element_Generic_Data _data)
        {
            GameObject graphicObj = Instantiate(_data.ElementPrefab);
            CreatePreviewObjectFromData(_data, new Vector3(5f, 5f, 5f), graphicObj.transform);
            graphicObj.transform.parent = _parent;
            graphicObj.transform.position = _parent.transform.position;

            I_GDR_Equippable equippable = null;

            if (_data.GetType() == typeof(WeaponData))
            {
                equippable = graphicObj.AddComponent<Weapon>();
                (equippable as Weapon).Init(_data);
            }
            else if (_data.GetType() == typeof(PotionData))
            {
                equippable = graphicObj.AddComponent<Potion>();
                (equippable as Potion).Init(_data);
            }
            else if (_data.GetType() == typeof(ArmorData))
            {
                equippable = graphicObj.AddComponent<Armor>();
                (equippable as Armor).Init(_data);
            }

            return equippable;
        }

        GameObject CreatePreviewObjectFromData(GDR_Element_Generic_Data _data, Vector3 _previewObjScale, Transform _parent)
        {
            GameObject previewObj = Instantiate(_data.ElementPrefab, _parent.position, _parent.rotation, _parent);
            foreach (Transform obj in previewObj.GetComponentsInChildren<Transform>())
            {
                obj.gameObject.layer = 9; // Preview Layer
            }
            previewObj.transform.localScale = _previewObjScale;

            return previewObj;
        }
        #endregion
    }
}