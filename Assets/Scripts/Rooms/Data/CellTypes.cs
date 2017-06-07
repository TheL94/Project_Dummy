using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Rooms.Cells.Data
{
    [CreateAssetMenu(fileName = "CellTypes", menuName = "Cell/NewCellTypes", order = 1)]
    public class CellTypes : ScriptableObject
    {
        public GameObject CellEmpty;
        public GameObject CellWallAngular;
        public GameObject CellWallBack;
        public GameObject CellWallColumnedAngular;
        public GameObject CellWallLeftRight;
        public GameObject CellWallOpenFront;
    }
}