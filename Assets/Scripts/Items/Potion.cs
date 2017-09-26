using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
using Framework.AI;

namespace DumbProject.Items
{
    public class Potion : ItemGeneric
    {
        public PotionData PotionValues;
        public override void Init(GenericDroppableData _values)
        {
            PotionValues = _values as PotionData;
            PotionValues.Type = GenericType.Item;
        }

        public override void Interact(AI_Controller _controller)
        {
            base.Interact(_controller);
            Destroy(gameObject);
        }
    }
}