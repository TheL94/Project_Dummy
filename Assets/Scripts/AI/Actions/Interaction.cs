using UnityEngine;
using System.Collections.Generic;
using DumbProject.Generic;
using DumbProject.Items;
using DumbProject.Rooms;
using Framework.Pathfinding;
using Framework.AI;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Action/Interaction")]
    public class Interaction : Action
    {
        public List<GenericType> PossibleInteraction = new List<GenericType>();

        public override void Act(AIController _controller)
        {
            Interact(_controller);   
        }

        void Interact(AIController _controller)
        {
            IInteractable interaction = Converter.INetworkableToIInteractable((_controller as AIActor).INetworkableObjective);
            if(interaction != null && interaction.IsInteractable)
            {
                Edge _edg = (interaction as MonoBehaviour).GetComponent<Edge>();
                if (_edg != null && _edg.Type == EdgeType.Door)
                {                    
                    CrossDoor((_controller as AIActor), _edg);
                    return;
                }
                foreach (GenericType type in PossibleInteraction)
                {
                    ItemGeneric item = (interaction as MonoBehaviour).GetComponent<ItemGeneric>();
                    if(item != null && item.Data.Type == type)
                    {
                        interaction.Interact((_controller as AIActor));
                        return;
                    }
                }
            }
        }

        void CrossDoor(AIActor _actor, Edge _doorToCross)
        {
            _doorToCross.Interact(_actor);
            _actor.MoveToNextPathNode();
        }
    }
}
