using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class Weapon : ItemGeneric
    {
        public WeaponData SwordValues;
        public override void Init(GenericItemData _values)
        {
            SwordValues = _values as WeaponData;
        }

        public override void Interact()
        {
            IsInteractable = false;
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