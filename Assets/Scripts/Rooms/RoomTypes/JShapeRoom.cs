using System;
using System.Collections;
using System.Collections.Generic;
using DumbProject.Grid;
using DumbProject.Rooms.Data;
using DumbProject.Rooms.Cells;
using UnityEngine;

namespace DumbProject.Rooms
{
    public class JShapeRoom : Room
    {
        protected override void PlaceCells(RoomData _data, GridController _grid)
        {
            GridNode centerNode = _grid.GetGridCenter();

            Cell centralCell = Instantiate(_data.CellPrefab).PlaceCell(centerNode, _grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x + 1, centerNode.GridPosition.z)).WorldPosition, this);
            Cell leftCell = Instantiate(_data.CellPrefab).PlaceCell(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x - 1, centerNode.GridPosition.z)), centralCell.transform, this);
            Cell leftTopCell = Instantiate(_data.CellPrefab).PlaceCell(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x - 1, centerNode.GridPosition.z + 1)), leftCell.transform, this);
            Cell rightCell = Instantiate(_data.CellPrefab).PlaceCell(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x + 1, centerNode.GridPosition.z)), centralCell.transform, this);     
            
            CellsInRoom = new List<Cell>() { centralCell, leftTopCell, centralCell, rightCell };
        }
    }
}