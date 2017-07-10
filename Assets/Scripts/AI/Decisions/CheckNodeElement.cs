using DumbProject.Grid;
using DumbProject.Items;
using UnityEngine;
using Framework.AI;
using DumbProject.Generic;

namespace DumbProject.AI {
    [CreateAssetMenu(menuName = "AI/State/Decision/CheckNodeElement")]
    public class CheckNodeElement : Decision
    {
        public override int Decide(AIController _controller)
        {
            IInteractable cellElement = (_controller as AIActor).Grid.GetSpecificGridNode(_controller.transform.position).RelativeCell.ActualInteractable;
            if (cellElement != null && cellElement.IsInteractable
                && Vector3.Distance(cellElement.Transf.position, _controller.transform.position) <= (_controller as AIActor).InteractionRadius)
            {
                (_controller as AIActor).InteractableObjective = cellElement;
                return 1;
            }
            else
                return 0;
        }
    }
}
