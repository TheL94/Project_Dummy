﻿using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject;
using DumbProject.Grid;
using Framework.Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/State/NewAction/Interact")]
    public class AI_Interact : AI_Action
    {
        protected override bool Act(AI_Controller _controller)
        {
            INetworkable objPosition = (_controller as Dumby).CurrentNetworkable;
            IInteractable obj = Converter.INetworkableToIInteractable(objPosition);

            //if(objPosition.GetType() == typeof(NetNode))
            //{
            //    obj = (objPosition as NetNode).RelativeInteractable;
            //}
            //else if(objPosition.GetType() == typeof(Cell))
            //{
            //    obj = (objPosition as Cell).ActualInteractable;
            //}

            if(obj == null)
            {
                Debug.LogWarning("No IInteractable to interact with found");
                return true;
            }
            else if (!obj.IsInteractable)
            {
                Debug.LogWarning("IInteractable already interacted. Can't interact more.");
                return true;
            }

            obj.Interact(_controller);
            return true;
        }
    }
}
