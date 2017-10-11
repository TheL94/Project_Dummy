using System.Collections.Generic;
using DumbProject.Rooms;
using DumbProject.Generic;
using DumbProject.GDR_System;
using Framework.Pathfinding;
using UnityEngine;

namespace Framework.AI
{
    [CreateAssetMenu(menuName = "AI/NewAction/FinalObjectiveReached")]
    public class AI_FinalObjectiveReached : AI_Action
    {
        protected override bool Act(AI_Controller _controller)
        {
            //Momentaneamente prendiamo come condizione finale l'aver ragiunto l'ultima stanza.
            if (GameManager.I.DungeonMng.ObjectiveRoom.StatusOfExploration == ExplorationStatus.InExploration)
                return true;
            else
                return false;
        }
    }
}