using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DumbProject.Grid;

namespace DumbProject.Generic
{
    public class RoomPreviewController : MonoBehaviour
    {

        public List<Transform> RoomSpawns = new List<Transform>();
        List<GridController> GridControllers = new List<GridController>();

        public void Init()
        {
            GridControllers = GetComponentsInChildren<GridController>().ToList();

            InitGrids();
        }

        void InitGrids()
        {
            foreach (GridController grid in GridControllers)
                grid.Setup();
        }
    }
}