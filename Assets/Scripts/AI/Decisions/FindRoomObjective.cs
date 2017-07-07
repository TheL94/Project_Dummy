using Framework.AI;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.Items;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.AI {
    [CreateAssetMenu(menuName = "AI/State/Decision/FindRoomObjective")]
    public class FindRoomObjective : Decision
    {
        public List<GenericType> ObjectivesPriority = new List<GenericType>();

        public override int Decide(AIController _controller)
        {
            return ChooseWhatToDo(_controller);
        }

        /// <summary>
        /// Ritorna 0 se viene trovato uno degli oggetti (in ordine di priorità) nella stanza
        /// Ritorna 1 se nella stanza non c'è nulla con cui interagire
        /// </summary>
        /// <param name="_controller"></param>
        /// <returns></returns>
        int ChooseWhatToDo(AIController _controller)
        {
            Room actualRoom = (_controller as AIActor).CurrentRoom;
            List<IDroppable> droppablesInRoom = actualRoom.DroppableList;

            foreach (GenericType priority in ObjectivesPriority)
            {
                foreach (IDroppable drop in droppablesInRoom)
                {
                    if(priority == drop.Data.Type)
                    {
                        (_controller as AIActor).NextRoomObjective = drop;
                        return 0;
                    }                
                }
            }
            actualRoom.StatusOfExploration = ExplorationStatus.Explored;
            return 1;
        }
    }
}
