using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Items
{
    public class Armor : ItemGeneric
    {
        public ArmorData ArmoryValues;
        public override void Init(GenericItemData _values)
        {
            ArmoryValues = _values as ArmorData;
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