using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    public GridPosition GridPosition;
    public Vector3 WorldPosition;
    public Cell RelativeCell;

    List<GridNode> AdjacentNodes;

    public GridNode() { }

    public GridNode(GridPosition _gridPosition, Vector3 _worldPosition)
    {
        GridPosition = _gridPosition;
        WorldPosition = _worldPosition;
    }

    public void Init(List<GridNode> _adjacentNodes)
    {
        AdjacentNodes = _adjacentNodes;
    }
}

public struct GridPosition
{
    public int x;
    public int y;

    public GridPosition(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}
