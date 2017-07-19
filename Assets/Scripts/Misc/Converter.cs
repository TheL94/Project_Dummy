using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.Grid;
using Framework.Pathfinding;

namespace DumbProject
{
    public static class Converter
    {
        public static IInteractable INetworkableToIInteractable(INetworkable _networkable)
        {
            IInteractable interactable = null;

            if (_networkable.GetType() == typeof(Door))
                interactable = _networkable as IInteractable;
            else if (_networkable.GetType() == typeof(GridNode) && (_networkable as GridNode).RelativeCell!= null)
                interactable = (_networkable as GridNode).RelativeCell.ActualInteractable;

            return interactable;
        }

        public static INetworkable IInteractableToINetworkable(IInteractable _interactable)
        {
            INetworkable networkable = null;

            if (_interactable.GetType() == typeof(Door))
                networkable = _interactable as INetworkable;
            else
            {
                try
                {
                    networkable = GameManager.I.MainGridCtrl.GetSpecificGridNode(_interactable.Transf.position);
                }
                catch
                {
                    return null;
                }
            }

            return networkable;
        }
    }
}

