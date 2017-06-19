﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms.Data;
using DumbProject.Rooms.Cells;
namespace DumbProject.Rooms
{
    public class TShapeRoom : Room
    {

        protected override void PlaceCells(RoomData _data, GridController _grid)
        {
            GridNode centerNode = _grid.GetGridCenter();
            Cell centralCell = Instantiate(_data.CellPrefab).PlaceCell(centerNode, transform, this);
            Cell centralTopCell = Instantiate(_data.CellPrefab).PlaceCell(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x, centerNode.GridPosition.z + 1)), centralCell.transform, this);
            Cell leftTopCell = Instantiate(_data.CellPrefab).PlaceCell(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x - 1, centerNode.GridPosition.z + 1)), centralTopCell.transform, this);
            Cell rightTopCell = Instantiate(_data.CellPrefab).PlaceCell(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x + 1, centerNode.GridPosition.z + 1)), centralTopCell.transform, this);
            
            CellsInRoom = new List<Cell>() { centralCell, centralTopCell, leftTopCell, rightTopCell };
        }
    }
}