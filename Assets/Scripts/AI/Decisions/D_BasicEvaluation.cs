using Framework.AI;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.Items;
using System.Collections.Generic;

namespace DumbProject.AI {
    public class D_BasicEvaluation : Decision
    {
        public override int Decide(AIController _controller)
        {
            return ChooseWhatToDo(_controller);
        }

        int ChooseWhatToDo(AIController _controller)
        {
            Room actualRoom = (_controller as Dumby).CurrentRoom;
            List<IDroppable> droppablesInRoom = actualRoom.DroppableList;

            foreach (IDroppable drop in droppablesInRoom)
            {
                //drop.Type
            }
        }
    }
}
