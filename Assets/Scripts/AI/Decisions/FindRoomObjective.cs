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
            List<IInteractable> interactableInRoom = actualRoom.InteractableAvailable;

            foreach (GenericType priority in ObjectivesPriority)
            {
                foreach (IInteractable interaction in interactableInRoom)
                {
                    ItemGeneric item = (interaction as MonoBehaviour).GetComponent<ItemGeneric>();
                    if (item == null)
                        continue;

                    if (priority == item.Data.Type && item.IsInteractable)
                    {
                        (_controller as AIActor).INetworkableObjective = Converter.IInteractableToINetworkable(interaction);
                        return 0;
                    }                
                }
            }
            actualRoom.StatusOfExploration = ExplorationStatus.Explored;
            return 1;
        }
    }
}
