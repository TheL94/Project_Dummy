using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int GridHeight;
    public int GridWidth;
    public float CellSize;
    public float CellOffset;

    public GameObject CellImage;

    GridNode[,] Grid;

    public void Setup()
    {
        CreateGrid(GridHeight, GridWidth, CellSize, CellOffset);

        if(CellImage != null)
            ShowGrid(CellImage, GridHeight, GridWidth);

        InitGridNodes(GridHeight, GridWidth);
    }
    public GridNode GetGridCenter()
    {     
        return Grid[GridHeight / 2, GridWidth / 2];
    }

    public GridNode GetSpecificGridNode(GridPosition _gridPosition)
    {
        return Grid[_gridPosition.x, _gridPosition.y];
    }

    public GridNode GetSpecificGridNode(Vector3 _worldPosition)
    {
        for (int i = 0; i < GridHeight; i++)
        {
            for (int j = 0; j < GridWidth; j++)
            {
                if (Vector2.Distance(Grid[i, j].WorldPosition, _worldPosition) < 0.25f)
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

    void CreateGrid(int _gridHeight, int _gridWidth, float _cellSize, float _cellOffset)
    {
        Grid = new GridNode[_gridHeight, _gridWidth];

        for (int i = 0; i < _gridHeight; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                Grid[i, j] = new GridNode(new GridPosition(i, j), new Vector3(transform.position.x + i * _cellSize + _cellOffset, transform.position.y + j * _cellSize + _cellOffset, 0));
            }
        }
    }

    void InitGridNodes(int _gridHeight, int _gridWidth)
    {
        for (int i = 0; i < _gridHeight; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                Grid[i, j].Init(GetAdjacentNodes(new GridPosition(i, j)));
            }
        }
    }


    void ShowGrid(GameObject _cellImage, int _gridHeight, int _gridWidth)
    {
        for (int i = 0; i < _gridHeight; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                Instantiate(_cellImage, Grid[i, j].WorldPosition, Quaternion.identity, transform);
            }
        }
    }

    List<GridNode> GetAdjacentNodes(GridPosition _gridPosition)
    {
        List<GridNode> adjacentNodes = new List<GridNode>();

        if (_gridPosition.x + 1 < GridHeight)
        {
            adjacentNodes.Add(Grid[_gridPosition.x + 1, _gridPosition.y]);
        }
        if (_gridPosition.x - 1 >= 0)
        {
            adjacentNodes.Add(Grid[_gridPosition.x - 1, _gridPosition.y]);
        }

        if (_gridPosition.y + 1 < GridWidth)
        {
            adjacentNodes.Add(Grid[_gridPosition.x, _gridPosition.y + 1]);
        }
        if (_gridPosition.y - 1 >= 0)
        {
            adjacentNodes.Add(Grid[_gridPosition.x, _gridPosition.y - 1]);
        }

        return adjacentNodes;
    }
}