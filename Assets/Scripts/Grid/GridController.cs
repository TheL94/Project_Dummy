using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int GridHeight;
    public int GridWidth;
    public float CellSize;
    public float CellOffset;

    public float GridPositionSensitivity;

    public GameObject CellImage;

    GridNode[,] Grid;

    public void Setup()
    {
        CreateGrid(GridWidth, GridHeight, CellSize, CellOffset);

        if (CellImage != null)
            ShowGrid(CellImage, GridWidth, GridHeight);

        InitGridNodes(GridWidth, GridHeight);
    }

    public GridNode GetGridCenter()
    {     
        return Grid[GridWidth / 2, GridHeight / 2];
    }

    public GridNode GetSpecificGridNode(GridPosition _gridPosition)
    {
        return Grid[_gridPosition.x, _gridPosition.z];
    }

    public GridNode GetSpecificGridNode(Vector3 _worldPosition)
    {
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                if (Vector2.Distance(Grid[i, j].WorldPosition, _worldPosition) < GridPositionSensitivity)
                    return Grid[i, j];
            }
        }
        return null;
    }

    public bool CheckAdjacentNodesRelativeCell(GridNode _node)
    {
        foreach (GridNode node in _node.AdjacentNodes)
        {
            if (node.RelativeCell != null)
            {
                return true;
            }
        }
        return false;
    }

    void CreateGrid(int _gridWidth, int _gridHeight, float _cellSize, float _cellOffset)
    {
        Grid = new GridNode[_gridWidth, _gridHeight];

        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                Grid[i, j] = new GridNode(new GridPosition(i, j), new Vector3(transform.position.x + i * _cellSize + _cellOffset, 0, transform.position.z + j * _cellSize + _cellOffset));
            }
        }
    }

    void InitGridNodes(int _gridWidth, int _gridHeight)
    {
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                Grid[i, j].Init(GetAdjacentNodes(new GridPosition(i, j)));
            }
        }
    }

    void ShowGrid(GameObject _cellImage, int _gridWidth, int _gridHeight)
    {
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                Instantiate(_cellImage, Grid[i, j].WorldPosition, Quaternion.identity, transform);
            }
        }
    }

    List<GridNode> GetAdjacentNodes(GridPosition _gridPosition)
    {
        List<GridNode> adjacentNodes = new List<GridNode>();

        if (_gridPosition.x + 1 < GridWidth)
        {
            adjacentNodes.Add(Grid[_gridPosition.x + 1, _gridPosition.z]);
        }
        if (_gridPosition.x - 1 >= 0)
        {
            adjacentNodes.Add(Grid[_gridPosition.x - 1, _gridPosition.z]);
        }

        if (_gridPosition.z + 1 < GridHeight)
        {
            adjacentNodes.Add(Grid[_gridPosition.x, _gridPosition.z + 1]);
        }
        if (_gridPosition.z - 1 >= 0)
        {
            adjacentNodes.Add(Grid[_gridPosition.x, _gridPosition.z - 1]);
        }

        return adjacentNodes;
    }
}