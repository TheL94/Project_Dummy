using System.Collections;
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
            Cell centralCell = Instantiate(_data.CellPrefab).PlaceCellInUI(centerNode, Quaternion.identity, transform);
            Cell leftTopCell = Instantiate(_data.CellPrefab).PlaceCellInUI(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x - 1, centerNode.GridPosition.z + 1)), Quaternion.identity, transform);
            Cell centralTopCell = Instantiate(_data.CellPrefab).PlaceCellInUI(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x, centerNode.GridPosition.z + 1)), Quaternion.identity, transform);
            Cell rightTopCell = Instantiate(_data.CellPrefab).PlaceCellInUI(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x + 1, centerNode.GridPosition.z + 1)), Quaternion.identity, transform);

            CellsInRoom = new List<Cell>() { centralCell, leftTopCell, centralTopCell, rightTopCell };

            Instantiate(_data.CellTypes.CellWallOpenFront, centralCell.transform.position, centralCell.transform.rotation, centralCell.transform);
            Instantiate(_data.CellTypes.CellWallOpenFront, leftTopCell.transform.position, leftTopCell.transform.rotation, leftTopCell.transform);
            Instantiate(_data.CellTypes.CellWallBack, centralTopCell.transform.position, centralTopCell.transform.rotation, centralTopCell.transform);
            Instantiate(_data.CellTypes.CellWallOpenFront, rightTopCell.transform.position, rightTopCell.transform.rotation, rightTopCell.transform);
        }

        public override void PlaceAsMainRoom(RoomData _data, GridController _grid)
        {
            GridNode centerNode = _grid.GetGridCenter();
            Cell centralCell = Instantiate(_data.CellPrefab).PlaceCellInMainGrid(centerNode);
            Cell leftTopCell = Instantiate(_data.CellPrefab).PlaceCellInMainGrid(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x - 1, centerNode.GridPosition.z + 1)));
            Cell centralTopCell = Instantiate(_data.CellPrefab).PlaceCellInMainGrid(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x, centerNode.GridPosition.z + 1)));
            Cell rightTopCell = Instantiate(_data.CellPrefab).PlaceCellInMainGrid(_grid.GetSpecificGridNode(new GridPosition(centerNode.GridPosition.x + 1, centerNode.GridPosition.z + 1)));

            CellsInRoom = new List<Cell>() { centralCell, leftTopCell, centralTopCell, rightTopCell };

            Instantiate(_data.CellTypes.CellWallOpenFront, centralCell.transform.position, centralCell.transform.rotation, centralCell.transform);
            Instantiate(_data.CellTypes.CellWallOpenFront, leftTopCell.transform.position, leftTopCell.transform.rotation, leftTopCell.transform);
            Instantiate(_data.CellTypes.CellWallBack, centralTopCell.transform.position, centralTopCell.transform.rotation, centralTopCell.transform);
            Instantiate(_data.CellTypes.CellWallOpenFront, rightTopCell.transform.position, rightTopCell.transform.rotation, rightTopCell.transform);
        }
    }
}