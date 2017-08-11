using System.Collections.Generic;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.Items;
using Framework.Pathfinding;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/State/NewAction/ChooseObjective")]
    public class AI_ChooseObjective : AI_Action
    {
        public List<GenericType> PriorityList = new List<GenericType>();

        protected override bool Act(AI_Controller _controller)
        {
            Room room = (_controller as Dumby).CurrentRoom;
            IInteractable objective = SearchInRoom(room);

            if (objective == null)
                objective = SearchInDungeon((_controller as Dumby).CurrentNetworkable);

            if (objective != null)
            {
                //Try to set as a NetNode in case of Doors
                (_controller as Dumby).Objective = GameManager.I.MainGridCtrl.GetSpecificNetNode(objective.Transf.position);
                //If still null, try to go for a NormalNode
                if((_controller as Dumby).Objective == null)
                    (_controller as Dumby).Objective = GameManager.I.MainGridCtrl.GetSpecificGridNode(objective.Transf.position);

                return true;
            }
            else
                return false;

        }

        IInteractable SearchInRoom(Room _room)
        {
            foreach (GenericType priorityType in PriorityList)
            {
                foreach (IInteractable avaibleItem in _room.InteractableAvailable)
                {
                    ItemGeneric item = (avaibleItem as MonoBehaviour).GetComponent<ItemGeneric>();
                    if(item != null)
                    {
                        if (item.Data.Type == priorityType)
                            return avaibleItem;
                    }
                }
            }

            return null;
        }

        IInteractable SearchInDungeon(INetworkable _actualNode)
        {
            List<Door> doorsToExplore = GameManager.I.DungeonMng.GetAllUnexploredDoors();
            int distancesInNodes = GameManager.I.MainGridCtrl.GridHeight * GameManager.I.MainGridCtrl.GridWidth;
            IInteractable closestDoor = null;

            if (doorsToExplore.Count <= 0)
                return null;

            foreach (Door door in doorsToExplore)
            {
                int dis = Pathfinder.FindPath(door.RelativeNetNode, _actualNode).Count;
                if(closestDoor == null)
                {
                    distancesInNodes = dis;
                    closestDoor = door;
                }
                else
                {
                    if(dis < distancesInNodes)
                    {
                        distancesInNodes = dis;
                        closestDoor = door;
                    }
                }
            }

            return closestDoor;
        }
    }
}


