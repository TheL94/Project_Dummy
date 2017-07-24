using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        List<NetNode> NetNodes = new List<NetNode>();
 
        #region API
        /// <summary>
        /// Setup della gliglia
        /// </summary>
        public void Setup()
        {
            CreateGrid(CellSize, CellOffset);

            if (CellImage != null)
                ShowGrid(CellImage);

            InitGridNodes();
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
            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    if (Vector3.Distance(Grid[i, j].WorldPosition, _worldPosition) < CellSize)
                        return Grid[i, j];
                }
            }
            return null;
        }

        /// <summary>
        /// Funzione che ritorna la griglia in formato lista
        /// </summary>
        /// <returns></returns>
        public List<GridNode> GridToList()
        {
            List<GridNode> listOfNodes = new List<GridNode>();
            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    listOfNodes.Add(Grid[i, j]);
                }
            }
            return listOfNodes;
        }

        /// <summary>
        /// Funzione che distrugge la griglia
        /// </summary>
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

        #region Net Node
        /// <summary>
        /// Funzione che crea eaggiunge un nuovo net node
        /// </summary>
        /// <param name="_position"></param>
        /// <param name="_adjacentNodes"></param>
        public void AddNewNetNode(Vector3 _position, List<NetNode> _adjacentNodes)
        {
            NetNode newNode = new NetNode(this, _position);
            newNode.Init(_adjacentNodes);
            NetNodes.Add(newNode);
        }

        /// <summary>
        /// Funzione che ritorna un net node in base alla posizione
        /// </summary>
        /// <param name="_worldPosition"></param>
        /// <returns></returns>
        public NetNode GetSpecificNetNode(Vector3 _worldPosition)
        {
            foreach (NetNode node in NetNodes)
            {
                if (node.WorldPosition == _worldPosition)
                    return node;
            }
            return null;
        }
        #endregion

        #region Cell
        // TODO : da spostare per aggiungere la classe al framework
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

        /// <summary>
        /// Funzione che canella i riferimenti della cella relativa dei nodi
        /// </summary>
        public void ClearNodesRelativeCell()
        {
            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    Grid[i, j].RelativeCell = null;
                }
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Crea la griglia in base ai parametri che gli sono passati
        /// </summary>
        /// <param name="_cellSize"></param>
        /// <param name="_cellOffset"></param>
        void CreateGrid(float _cellSize, float _cellOffset)
        {
            Grid = new GridNode[GridWidth, GridHeight];

            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    Grid[i, j] = new GridNode(this, new GridPosition(i, j), new Vector3(transform.position.x + i * _cellSize + _cellOffset, transform.position.y, transform.position.z + j * _cellSize + _cellOffset));
                }
            }
        }
        
        /// <summary>
        /// Inizializza i nodi della gliglia assegnando loro i riferimenti ai propri nodi adiacenti
        /// </summary>
        void InitGridNodes()
        {
            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    Grid[i, j].Init(GetAdjacentNodes(new GridPosition(i, j)));
                }
            }
        }

        /// <summary>
        /// Crea l'elemento visivo per mostrare i nodi in editor
        /// </summary>
        /// <param name="_cellImage"></param>
        void ShowGrid(GameObject _cellImage)
        {
            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
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
        List<NetNode> GetAdjacentNodes(GridPosition _gridPosition)
        {
            List<NetNode> adjacentNodes = new List<NetNode>();

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