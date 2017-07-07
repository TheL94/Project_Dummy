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
            IDroppable cellElement = (_controller as AIActor).Grid.GetSpecificGridNode(_controller.transform.position).RelativeCell.ActualDroppable;
            if (cellElement != null && Vector3.Distance(cellElement.transF.position, _controller.transform.position) <= (_controller as AIActor).InteractionRadius)
                return 1;
            else
                return 0;
        }
    }
}
