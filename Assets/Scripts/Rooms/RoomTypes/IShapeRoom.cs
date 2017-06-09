﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms.Data;
using DumbProject.Rooms.Cells;

namespace DumbProject.Rooms
{
    public class IShapeRoom : Room
    {
        public override void PlaceAsMainRoom(RoomData _data, GridController _grid)
        {
            GridNode centerNode = _grid.GetGridCenter();
            Cell centralCell = Instantiate(_data.CellPrefab).PlaceCellInUI(centerNode, Quaternion.identity, transform);
            Cell leftCell = Instantiate(_data.CellPrefab).PlaceCellInUI(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x - 1, centerNode.GridPosition.z)), Quaternion.identity, transform);
            Cell rightCell = Instantiate(_data.CellPrefab).PlaceCellInUI(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x + 1, centerNode.GridPosition.z)), Quaternion.identity, transform);

            CellsInRoom = new List<Cell>() { centralCell, leftCell, rightCell };

            Instantiate(_data.CellTypes.CellWallOpenFront, centralCell.transform.position, centralCell.transform.rotation, centralCell.transform);
            Instantiate(_data.CellTypes.CellWallLeftRight, leftCell.transform.position, leftCell.transform.rotation, leftCell.transform);
            Instantiate(_data.CellTypes.CellWallOpenFront, rightCell.transform.position, rightCell.transform.rotation, rightCell.transform);
        }

        protected override void PlaceCells(RoomData _data, GridController _grid)
        {
            GridNode centerNode = _grid.GetGridCenter();
            Cell centralCell = Instantiate(_data.CellPrefab).PlaceCellInMainGrid(centerNode, Quaternion.identity, transform);
            Cell leftCell = Instantiate(_data.CellPrefab).PlaceCellInMainGrid(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x - 1, centerNode.GridPosition.z)), Quaternion.identity, transform);
            Cell rightCell = Instantiate(_data.CellPrefab).PlaceCellInMainGrid(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x + 1, centerNode.GridPosition.z)), Quaternion.identity, transform);

            CellsInRoom = new List<Cell>() { centralCell, leftCell, rightCell };

            Instantiate(_data.CellTypes.CellWallOpenFront, centralCell.transform.position, centralCell.transform.rotation, centralCell.transform);
            Instantiate(_data.CellTypes.CellWallLeftRight, leftCell.transform.position, leftCell.transform.rotation, leftCell.transform);
            Instantiate(_data.CellTypes.CellWallOpenFront, rightCell.transform.position, rightCell.transform.rotation, rightCell.transform);
        }
    }
}