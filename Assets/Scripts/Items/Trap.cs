using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.GDR;


namespace DumbProject.Items
{
    public class Trap : MonoBehaviour
    {
        public TrapData TrapValues;
        public Room RelativeRoom; // o forse Cell ??
        public bool IsActive { get; private set; }

        public void Init(GenericDroppableData _values, Room _relativeRoom)
        {
            TrapValues = _values as TrapData;
            TrapValues.Type = GenericType.Trap;
            RelativeRoom = _relativeRoom;
        }

        private void Update()
        {
            if (RelativeRoom.StatusOfExploration != ExplorationStatus.InExploration)
                return;

            if (Vector3.Distance(GameManager.I.Dumby.transform.position, transform.position) <= TrapValues.ActivationRadius)
                Action();
        }

        public void Action()
        {
            GDR_Controller controller = GameManager.I.Dumby.GetComponent<GDR_Controller>();
            controller.Data.GetDamage(TrapValues.Damage);
        }
    }
}
