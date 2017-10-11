using System.Collections.Generic;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.GDR_System;
using Framework.Pathfinding;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/NewAction/ChooseObjective")]
    public class AI_ChooseObjective : AI_Action
    {
        public List<GenericType> PriorityList = new List<GenericType>();
        public bool GameOverIfNothigFound = false;

        protected override bool Act(AI_Controller _controller)
        {
            Room room = GameManager.I.DungeonMng.ActualInExplorationRoom;
            IInteractable objective = SearchInRoom(room);

            if (objective == null)
            {
                GameManager.I.DungeonMng.UpdateRoomStatus(room, ExplorationStatus.Explored);
                objective = SearchInDungeon((_controller as Dumby).CurrentNetworkable);
            }

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
            {
                if (GameOverIfNothigFound)
                    GameManager.I.GameLost();
                return false;
            }

        }

        IInteractable SearchInRoom(Room _room)
        {
            foreach (GenericType priorityType in PriorityList)
            {
                foreach (IInteractable avaibleItem in _room.InteractableAvailable)
                {
                    // casting necessario
                    I_GDR_Interactable GDR_element = (avaibleItem as I_GDR_Interactable);
                    if(GDR_element != null)
                    {
                        if (GDR_element.GDR_Data.Type == priorityType)
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


