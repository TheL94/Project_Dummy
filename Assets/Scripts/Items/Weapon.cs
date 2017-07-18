using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Items
{
    public class Weapon : ItemGeneric
    {
        public WeaponData SwordValues;
        public override void Init(GenericItemData _values)
        {
            SwordValues = _values as WeaponData;
            SwordValues.Type = GenericType.Item;
        }

        private void OnDrawGizmos()
        {
            if (!IsInteractable)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawCube(transform.position, Vector3.one);
            }
        }
    }
}