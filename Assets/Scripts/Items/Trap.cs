using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items
{
    public class Trap : MonoBehaviour
    {
        public TrapData TrapValues;
        public void Init(GenericDroppableData _values)
        {
            TrapValues = _values as TrapData;
            TrapValues.Type = GenericType.Trap;
        }
    }
}
