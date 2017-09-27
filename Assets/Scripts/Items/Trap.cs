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
        public TrapData Data;
        public Cell RelativeCell;
        public bool IsActive { get; private set; }

        public void Init(GenericDroppableData _values, Cell _relativeCell)
        {
            Data = _values as TrapData;
            RelativeCell = _relativeCell;
        }

        private void Update()
        {
            if (RelativeCell.RelativeRoom.StatusOfExploration != ExplorationStatus.InExploration)
                return;

            if (Vector3.Distance(GameManager.I.Dumby.transform.position, transform.position) <= Data.ActivationRadius)
                Action();
        }

        public void Action()
        {
            GDR_Controller controller = GameManager.I.Dumby.GetComponent<GDR_Controller>();
            controller.Data.GetDamage(Data.Damage);
        }
    }
}
