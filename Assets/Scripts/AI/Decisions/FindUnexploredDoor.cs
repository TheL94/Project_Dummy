using Framework.AI;
using Framework.Pathfinding;
using DumbProject.Generic;
using DumbProject.Rooms;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Decision/FindUnexploredDoor")]
    public class FindUnexploredDoor : Decision
    {
        public override int Decide(AIController _controller)
        {
            return FindNextUnexploredDoor(_controller);
        }

        int FindNextUnexploredDoor(AIController _controller)
        {
            //List<IInteractable> unexploredDoors = GameManager.I.DungeonMng.GetAllUnexploredDoors().ConvertAll(e => e as IInteractable);
            List<IInteractable> unexploredDoors = new List<IInteractable>();
            foreach (Door door in GameManager.I.DungeonMng.GetAllUnexploredDoors())
            {
                unexploredDoors.Add(door as IInteractable);
            }

            if (unexploredDoors == null || unexploredDoors.Count <= 0)
            {
                Debug.LogWarning("NON CI SONO PORTE INESPLORATE DISPONIBILI. MUORI");
                return 1;
            }
            
            (_controller as AIActor).INetworkableObjective = Converter.IInteractableToINetworkable(unexploredDoors[Random.Range(0, unexploredDoors.Count)]);
            return 0;
        }
    }
}
