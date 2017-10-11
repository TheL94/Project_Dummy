using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.GDR;
using Framework.AI;

namespace DumbProject.GDR_System
{
    public class Trap : MonoBehaviour, IInteractable
    {
        public Cell RelativeCell;

        public TrapData Data { get; private set; }
        public bool IsInteractable { get; set; }
        public Transform Transf { get { return transform; } }

        public void Init(GDR_Element_Generic_Data _data)
        {
            Data = _data as TrapData;
            IsInteractable = false;
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
            if (controller.Data.GetDamage(Data.Damage))
            {
                controller.Data.GetExperience(ExperienceType.Trap, Data);
                //TODO : Da rivedere
            }
        }

        public void Interact(AI_Controller _controller)
        {

        }
    }
}
