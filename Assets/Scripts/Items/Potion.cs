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
        new public PotionData Data;
        public override void Init(GenericDroppableData _values)
        {
            Data = _values as PotionData;
            Data.Type = GenericType.Item;
        }

        public override void Interact(AI_Controller _controller)
        {
            base.Interact(_controller);
            Destroy(gameObject);
        }
    }
}