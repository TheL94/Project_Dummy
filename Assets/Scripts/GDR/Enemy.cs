using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Generic;
using Framework.AI;

namespace DumbProject.GDR
{
    public class Enemy : MonoBehaviour, IInteractable
    {

        public GDR_Controller gdrController;
        public Cell RelativeCell;
        public bool IsActive { get; private set; }

        public bool IsInteractable { get; set; }


        public Transform Transf { get { return transform; }}

        public void Init(Cell _relativeCell)
        {
            RelativeCell = _relativeCell;
        }


        public void Action()
        {
            if (gdrController.Data.Life <=0 || !IsInteractable)
            {
                IsInteractable = false;
                return;
            }
            GDR_Controller controller = GameManager.I.Dumby.GetComponent<GDR_Controller>();
            if (controller.Data.GetDamage(gdrController.Data.Attack))
            {
                controller.Data.GetExperience(ExperienceType.Enemy);
                //TODO : Da riposizionare(forse)
            }
        }

        public void Interact(AI_Controller _controller)
        {
            Action();
        }
    }
}