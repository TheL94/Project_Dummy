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
            grid = GameManager.I.MainGridCtrl; ;
            animator = GetComponentInChildren<Animator>();
            base.Setup(_setActive);
        }

        protected override void OnUpdate()
        {
            FollowPath();
        }
    }
}