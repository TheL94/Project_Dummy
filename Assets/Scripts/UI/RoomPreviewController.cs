using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;

namespace DumbProject.Generic
{
    public class RoomPreviewController : MonoBehaviour
    {
        List<GridController> _gridCtrls = new List<GridController>();

        public List<GridController> GridCtrls { get { return _gridCtrls; } }

        public void Setup()
        {
            foreach (GridController gridCtrl in GetComponentsInChildren<GridController>())
            {
                if(!GridCtrls.Contains(gridCtrl))
                    GridCtrls.Add(gridCtrl);
            }
            SetupGrids();
        }

        void SetupGrids()
        {
            foreach (GridController grid in GridCtrls)
                grid.Setup();
        }

        public void DestroyUIGrid()
        {
            foreach (GridController gridCtrl in GridCtrls)
                gridCtrl.DestroyGrid();
        }
    }
}