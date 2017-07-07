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
            List<INetworkable> unexploredDoors = GameManager.I.DungeonMng.GetAllUnexploredDoors().ConvertAll(e => e as INetworkable);

            if (unexploredDoors == null || unexploredDoors.Count <= 0)
                return 1;
            
            (_controller as AIActor).NextObjectivePosition = unexploredDoors[Random.Range(0, unexploredDoors.Count)];
            return 0;
        }
    }
}
