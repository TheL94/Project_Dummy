using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms;
using DumbProject.Items;
using Framework.AI;
using Framework.Pathfinding;
using DG.Tweening;

namespace DumbProject.Generic
{
    public class Dumby : AIActor
    {
        public override void Setup(bool _setActive = true)
        {
            Grid = GameManager.I.MainGridCtrl; ;
            animator = GetComponentInChildren<Animator>();
            base.Setup(_setActive);
        }

        public override void BlockPathWithObstacles()
        {
            for (int i = 0; i < nodePath.Count; i++)
            {
                if (nodePath[i].GetType() != typeof(Edge))
                {
                    if (Grid.GetSpecificGridNode(nodePath[i].spacePosition).RelativeCell.ActualInteractable != null)
                    {
                        nodePath.RemoveRange(i + 1, nodePath.Count - i - 1);
                        break;
                    }
                }
            }            
        }
    }
}