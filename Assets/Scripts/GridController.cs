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
    }

    public GridNode GetGridCenter()
    {     
        return Grid[GridHeight / 2, GridWidth / 2];
    }

    public GridNode GetSpecificGridNode(GridPosition _gridPosition)
    {
        return Grid[_gridPosition.x, _gridPosition.y];
    }

    public GridNode GetAdjacentPositions(GridNode _node, Directions _direction)
    {
        GridNode adjacentPosition = new GridNode();
        switch (_direction) 
        {
            case Directions.Up:
                adjacentPosition = Grid[_node.GridPosition.x, _node.GridPosition.y + 1];
                break;
            case Directions.Down:
                adjacentPosition = Grid[_node.GridPosition.x, _node.GridPosition.y - 1];
                break;
            case Directions.Left:
                adjacentPosition = Grid[_node.GridPosition.x - 1, _node.GridPosition.y];
                break;
            case Directions.Rigth:
                adjacentPosition = Grid[_node.GridPosition.x + 1, _node.GridPosition.y];
                break;
        }
        return adjacentPosition;
    }

    void CreateGrid(int _gridHeight, int _gridWidth, float _cellSize, float _cellOffset)
    {
        Grid = new GridNode[_gridHeight, _gridWidth];

        for (int i = 0; i < _gridHeight; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                Grid[i, j] = new GridNode(new GridPosition(i, j), new Vector3(transform.position.x + i * _cellSize + _cellOffset, transform.position.y + j * _cellSize + _cellOffset, 0), GridNode.CellState.Available);
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

    public enum Directions {Up, Down, Left, Rigth}
}