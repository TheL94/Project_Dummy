using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Cells;
using System.Linq;

namespace DumbProject.Grid
{
    public class GridController : MonoBehaviour
    {
        public int GridHeight;
        public int GridWidth;
        public float CellSize;
        public float CellOffset;
        public float GridOffsetY = 0f;
        public float GridPositionSensitivity;

        public GameObject CellImage;

        GridNode[,] Grid;

        #region API
        /// <summary>
        /// Setup della gliglia
        /// </summary>
        public void Setup()
        {
            CreateGrid(GridWidth, GridHeight, CellSize, CellOffset);

            if (CellImage != null)
                ShowGrid(CellImage, GridWidth, GridHeight);

            InitGridNodes(GridWidth, GridHeight);
        }

        /// <summary>
        /// Ritorna il nodo centrale della gliglia (Approssimato per difetto)
        /// </summary>
        /// <returns></returns>
        public GridNode GetGridCenter()
        {
            return Grid[GridWidth / 2, GridHeight / 2];
        }

        /// <summary>
        /// Ritorna un nodo specifico per posizione nella griglia
        /// </summary>
        /// <param name="_gridPosition"></param>
        /// <returns></returns>
        public GridNode GetSpecificGridNode(GridPosition _gridPosition)
        {
            return Grid[_gridPosition.x, _gridPosition.z];
        }

        /// <summary>
        /// Ritorna un  nodo specifico per posizione nel mondo
        /// </summary>
        /// <param name="_worldPosition"></param>
        /// <returns></returns>
        public GridNode GetSpecificGridNode(Vector3 _worldPosition)
        {
            if (_worldPosition == null)
                return null;

            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    if (Vector3.Distance(Grid[i, j].WorldPosition, _worldPosition) <= GridPositionSensitivity)
                        return Grid[i, j];
                }
            }
            return null;
        }

        /// <summary>
        /// Controlla la presenza di una cella su uno dei nodi adiacenti a quello passato come parametro
        /// </summary>
        /// <param name="_node"></param>
        /// <returns></returns>
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

        public void DestroyGrid()
        {
            Grid = null;
            List<Transform> childrenTransform = GetComponentsInChildren<Transform>().ToList();
            childrenTransform.Remove(transform);
            foreach (Transform item in childrenTransform)
            {
                Destroy(item.gameObject);
            }

        }
        #endregion

        /// <summary>
        /// Crea la griglia in base ai parametri che gli sono passati
        /// </summary>
        /// <param name="_gridWidth"></param>
        /// <param name="_gridHeight"></param>
        /// <param name="_cellSize"></param>
        /// <param name="_cellOffset"></param>
        void CreateGrid(int _gridWidth, int _gridHeight, float _cellSize, float _cellOffset)
        {
            Grid = new GridNode[_gridWidth, _gridHeight];

            for (int i = 0; i < _gridWidth; i++)
            {
                for (int j = 0; j < _gridHeight; j++)
                {
                    Grid[i, j] = new GridNode(this, new GridPosition(i, j), new Vector3(transform.position.x + i * _cellSize + _cellOffset, transform.position.y, transform.position.z + j * _cellSize + _cellOffset));
                }
            }
        }

        /// <summary>
        /// Inizializza i nodi della gliglia assegnando loro i riferimenti ai propri nodi adiacenti
        /// </summary>
        /// <param name="_gridWidth"></param>
        /// <param name="_gridHeight"></param>
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

        /// <summary>
        /// Crea l'elemento visivo per mostrare i nodi in editor
        /// </summary>
        /// <param name="_cellImage"></param>
        /// <param name="_gridWidth"></param>
        /// <param name="_gridHeight"></param>
        void ShowGrid(GameObject _cellImage, int _gridWidth, int _gridHeight)
        {
            for (int i = 0; i < _gridWidth; i++)
            {
                for (int j = 0; j < _gridHeight; j++)
                {
                    GameObject obj = Instantiate(_cellImage, Grid[i, j].WorldPosition, Quaternion.identity, transform);
                    obj.name = "Node[ " + i + ", " + j + " ]";
                }
            }
        }

        /// <summary>
        /// Ritorna i nodi adiacenti al nodo passato come parametro
        /// </summary>
        /// <param name="_gridPosition"></param>
        /// <returns></returns>
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
}