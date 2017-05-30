using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    public GridPosition GridPosition;
    public Vector3 WorldPosition;
    public CellState State;
    public Cell RelativeCell;

    public GridNode() { }

    public GridNode(GridPosition _gridPosition, Vector3 _worldPosition, CellState _state)
    {
        GridPosition = _gridPosition;
        WorldPosition = _worldPosition;
        State = _state;
    }

    public enum CellState { Available, Occupied }
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
