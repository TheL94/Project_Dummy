using Framework.AI;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.Items;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.AI {
    [CreateAssetMenu(menuName = "AI/State/Decision/BasicEvaluation")]
    public class D_BasicEvaluation : Decision
    {
        public override int Decide(AIController _controller)
        {
            return ChooseWhatToDo(_controller);
        }

        int ChooseWhatToDo(AIController _controller)
        {
            Room actualRoom = (_controller as AIActor).CurrentRoom;
            List<IDroppable> droppablesInRoom = actualRoom.DroppableList;
            int returnValue = 0;
            IDroppable objective = null;
            foreach (IDroppable drop in droppablesInRoom)
            {
                switch (drop.Data.Type)
                {
                    case ItemType.Ememy:
                        returnValue = 1;
                        objective = drop;
                        break;
                    case ItemType.Item:
                        if (returnValue == 0 || returnValue > 1)
                        {
                            returnValue = 2;
                            objective = drop;
                        }
                        break;
                    case ItemType.Trap:
                        break;
                    case ItemType.Gattini:
                        if(returnValue > 2)
                        {
                            returnValue = 3;
                            objective = drop;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (objective != null)
                (_controller as AIActor).nextRoomObjective = objective;

            returnValue = returnValue > 0 ? 1 : 0;
            return returnValue;
        }
    }
}
