﻿using System;
using System.Collections;
using System.Collections.Generic;
using DumbProject.Grid;
using DumbProject.Rooms.Data;
using DumbProject.Rooms.Cells;
using UnityEngine;

namespace DumbProject.Rooms
{
    public class OShapeRoom : Room
    {
        protected override void PlaceCells(RoomData _data, GridController _grid)
        {
            GridNode centerNode = _grid.GetGridCenter();

            Cell centralCell = Instantiate(_data.CellPrefab).PlaceCellInMainGrid(centerNode, Quaternion.identity, transform);
            Cell rightTopCell = Instantiate(_data.CellPrefab).PlaceCellInUI(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x + 1, centerNode.GridPosition.z + 1)), Quaternion.identity, transform);
            Cell centralTopCell = Instantiate(_data.CellPrefab).PlaceCellInMainGrid(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x, centerNode.GridPosition.z + 1)), Quaternion.identity, transform);
            Cell rightCell = Instantiate(_data.CellPrefab).PlaceCellInMainGrid(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x + 1, centerNode.GridPosition.z)), Quaternion.identity, transform);

            CellsInRoom = new List<Cell>() { centralCell, rightTopCell, centralCell, rightCell };

            Instantiate(_data.CellTypes.CellWallAngular, centralCell.transform.position, centralCell.transform.rotation, centralCell.transform);
            Instantiate(_data.CellTypes.CellWallAngular, rightTopCell.transform.position, rightTopCell.transform.rotation, rightTopCell.transform);
            Instantiate(_data.CellTypes.CellWallAngular, centralTopCell.transform.position, centralTopCell.transform.rotation, centralTopCell.transform);
            Instantiate(_data.CellTypes.CellWallAngular, rightCell.transform.position, rightCell.transform.rotation, rightCell.transform);
        }
    }
}