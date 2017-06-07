using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DumbProject.Grid;

namespace DumbProject.Generic
{
    public class RoomPreviewController : MonoBehaviour
    {
        List<GridSpawn> GridSpawns = new List<GridSpawn>();

        public void Setup()
        {
            foreach (GridController gridCtrl in GetComponentsInChildren<GridController>())
                GridSpawns.Add(new GridSpawn(gridCtrl, true));
            SetupGrids();
        }

        void SetupGrids()
        {
            foreach (GridSpawn gridSpawn in GridSpawns)
                gridSpawn.GridCtrl.Setup();
        }

        public GridController GetFirstGridAvailable()
        {
            foreach (GridSpawn gridSpawn in GridSpawns)
                if(gridSpawn.IsAvailable)
                    return gridSpawn.GridCtrl;
            return null;
        }
    }

    public class GridSpawn
    {
        public GridController GridCtrl;
        public bool IsAvailable;

        public GridSpawn(GridController _gridCtrl, bool _isAvailable)
        {
            GridCtrl = _gridCtrl;
            IsAvailable = _isAvailable;
        }
    }
}