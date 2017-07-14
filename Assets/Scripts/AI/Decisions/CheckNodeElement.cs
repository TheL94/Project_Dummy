using DumbProject.Rooms;
using System.Collections.Generic;
using UnityEngine;
using Framework.AI;
using DumbProject.Generic;

namespace DumbProject.AI {
    [CreateAssetMenu(menuName = "AI/State/Decision/CheckNodeElement")]
    public class CheckNodeElement : Decision
    {
        public override int Decide(AIController _controller)
        {
            return DecideCheck(_controller as AIActor);
        }

        int DecideCheck(AIActor _actor)
        {
            List<IInteractable> possibleInteractions = _actor.GetCurrentCellInteractables();
            foreach (IInteractable cellElement in possibleInteractions)
            {
                if (cellElement != null && cellElement.IsInteractable
                    && Vector3.Distance(cellElement.Transf.position, _actor.transform.position) <= _actor.InteractionRadius)
                {
                    foreach (var item in possibleInteractions)
                    {
                        Debug.Log((item as MonoBehaviour).name);
                    }
                    _actor.INetworkableObjective = Converter.IInteractableToINetworkable(cellElement);
                    return 1;
                }
            }

            return 0;
        }
    }
}
