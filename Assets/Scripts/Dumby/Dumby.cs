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
            for (int i = 0; i < NodePath.Count; i++)
            {
                if (NodePath[i].GetType() != typeof(Edge))
                {
                    IInteractable interactable = Grid.GetSpecificGridNode(NodePath[i].spacePosition).RelativeCell.ActualInteractable;
                    if (interactable != null && interactable.IsInteractable)
                    {
                        NodePath.RemoveRange(i + 1, NodePath.Count - i - 1);
                        break;
                    }
                }
            }            
        }

        private void OnDrawGizmos()
        {
            if (!DebugMode)
                return;
            if (CurrentState != null)
            {
                Gizmos.color = CurrentState.StateColor;
                Gizmos.DrawWireSphere(transform.position + new Vector3(0f, 2.5f, 0f), 0.5f);
            }


            if(INetworkableObjective != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(INetworkableObjective.spacePosition + new Vector3(0f, 8f, 0f), 0.5f);
            }

            if (NodePath == null)
                return;

            Gizmos.color = Color.magenta;
            foreach (INetworkable node in NodePath)
            {
                Gizmos.DrawWireCube(node.spacePosition, Vector3.one);
            }
        }
    }
}