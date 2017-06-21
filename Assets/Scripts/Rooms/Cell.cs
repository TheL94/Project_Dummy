using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Rooms.Cells.Doors;

namespace DumbProject.Rooms.Cells
{
    public class Cell : MonoBehaviour
    {
        private bool _isFree = true;
        /// <summary>
        /// se la cella ha già un oggetto assegnato
        /// </summary>
        public bool IsFree
        {
            get { return _isFree; }
            set { _isFree = value; }
        }

        GridNode _relativeNode;
        public GridNode RelativeNode
        {
            get { return _relativeNode; }
            set
            {
                if (_relativeNode != null && _relativeNode.RelativeCell != null)
                    _relativeNode.RelativeCell = null;

                _relativeNode = value;
                if (_relativeNode != null)
                {
                    transform.position = RelativeNode.WorldPosition;
                    RelativeNode.RelativeCell = this;
                }
            }
        }

        Room _relativeRoom;
        public Room RelativeRoom
        {
            get { return _relativeRoom; }
            private set { _relativeRoom = value; }
        }

        List<Cell> adjacentCells = new List<Cell>();

        GameObject Floor;
        List<GameObject> Edges = new List<GameObject>();
        List<GameObject> Angles = new List<GameObject>();
        List<Door> Doors = new List<Door>();

        #region Cell Elements Instantiation
        /// <summary>
        /// Crea il contenitore del pavimento
        /// </summary>
        void InstantiateFloor()
        {
            Floor = new GameObject("Floor");
            Floor.transform.localPosition = transform.position;
            Floor.transform.parent = transform;
            Floor.transform.rotation = transform.rotation;
        }

        /// <summary>
        /// Crea i contenitori dei muri
        /// </summary>
        void InstantiateEdges()
        {
            GameObject newEdge;
            Quaternion newRotation;
            int distance = (int)RelativeNode.RelativeGrid.CellSize / 2;

            newEdge = new GameObject("RightEdge");
            newEdge.transform.localPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
            newEdge.transform.parent = transform;
            newRotation = ((transform.position - newEdge.transform.position) != Vector3.zero) ? Quaternion.LookRotation(transform.position - newEdge.transform.position) : Quaternion.identity;
            newEdge.transform.rotation = newRotation;
            Edges.Add(newEdge);

            newEdge = new GameObject("LeftEdge");
            newEdge.transform.localPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
            newEdge.transform.parent = transform;
            newRotation = ((transform.position - newEdge.transform.position) != Vector3.zero) ? Quaternion.LookRotation(transform.position - newEdge.transform.position) : Quaternion.identity;
            newEdge.transform.rotation = newRotation;
            Edges.Add(newEdge);

            newEdge = new GameObject("UpEdge");
            newEdge.transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
            newEdge.transform.parent = transform;
            newRotation = ((transform.position - newEdge.transform.position) != Vector3.zero) ? Quaternion.LookRotation(transform.position - newEdge.transform.position) : Quaternion.identity;
            newEdge.transform.rotation = newRotation;
            Edges.Add(newEdge);

            newEdge = new GameObject("DownEdge");
            newEdge.transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
            newEdge.transform.parent = transform;
            newRotation = ((transform.position - newEdge.transform.position) != Vector3.zero) ? Quaternion.LookRotation(transform.position - newEdge.transform.position) : Quaternion.identity;
            newEdge.transform.rotation = newRotation;
            Edges.Add(newEdge);
        }

        /// <summary>
        /// Crea il contenitore dei pilastri
        /// </summary>
        void InstantiateAngles()
        {
            GameObject newAngle;
            int distance = (int)RelativeNode.RelativeGrid.CellSize / 2;

            newAngle = new GameObject("NE_Angle");
            newAngle.transform.localPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z + distance);
            newAngle.transform.parent = transform;
            Angles.Add(newAngle);

            newAngle = new GameObject("SE_Angle");
            newAngle.transform.localPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z - distance);
            newAngle.transform.parent = transform;
            Angles.Add(newAngle);

            newAngle = new GameObject("NO_Angle");
            newAngle.transform.localPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z + distance);
            newAngle.transform.parent = transform;
            Angles.Add(newAngle);

            newAngle = new GameObject("SO_Angle");
            newAngle.transform.localPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z - distance);
            newAngle.transform.parent = transform;
            Angles.Add(newAngle);
        }

        /// <summary>
        /// Istanzia l'oggeto di grafica del pavimento
        /// </summary>
        void InstantiateFloorElement()
        {
            GameObject newObj = null;
            newObj = Instantiate(RelativeRoom.Data.RoomElements.FloorPrefab, Floor.transform.position, Floor.transform.rotation, Floor.transform);
            newObj.tag = "Floor";
        }

        /// <summary>
        /// Istanzia l'oggeto di grafica del pilastro
        /// </summary>
        void InstantiatePilllarElements()
        {
            GameObject newObj = null;
            foreach (GameObject angle in Angles)
            {
                newObj = Instantiate(RelativeRoom.Data.RoomElements.PillarPrefab, angle.transform.position, Quaternion.identity, angle.transform);
                newObj.tag = "Angle";
            }
        }

        /// <summary>
        /// Istanzia l'oggeto di grafica del muro
        /// </summary>
        void InstantiateWallElements()
        {
            GameObject newObj = null;
            foreach (GameObject edge in Edges)
            {
                if (edge.name == "RightEdge" || edge.name == "LeftEdge")
                {
                    newObj = Instantiate(RelativeRoom.Data.RoomElements.WallPrefab, edge.transform.position, Quaternion.identity, edge.transform);
                    newObj.tag = "Edge";
                }
                else if (edge.name == "UpEdge")
                {
                    newObj = Instantiate(RelativeRoom.Data.RoomElements.WallPrefab, edge.transform.position, Quaternion.LookRotation(Angles.Find(a => a.name == "NE_Angle").transform.position - edge.transform.position), edge.transform);
                    newObj.tag = "Edge";
                }
                else if (edge.name == "DownEdge")
                {
                    newObj = Instantiate(RelativeRoom.Data.RoomElements.WallPrefab, edge.transform.position, Quaternion.LookRotation(Angles.Find(a => a.name == "SE_Angle").transform.position - edge.transform.position), edge.transform);
                    newObj.tag = "Edge";
                }
            }
        }
        #endregion

        #region API
        public Cell PlaceCell(GridNode _relativeNode, Transform _pointToFace, Room _relativeRoom)
        {
            Quaternion newRotation = ((_pointToFace.position - transform.position) != Vector3.zero) ? Quaternion.LookRotation(_pointToFace.position - transform.position) : Quaternion.identity;
            return PlaceCell(_relativeNode, newRotation, _relativeRoom);           
        }

        public Cell PlaceCell(GridNode _relativeNode, Vector3 _pointToFace, Room _relativeRoom)
        {
            Quaternion newRotation = Quaternion.LookRotation(_pointToFace - transform.position);
            return PlaceCell(_relativeNode, newRotation, _relativeRoom);
        }

        public Cell PlaceCell(GridNode _relativeNode, Quaternion _pointToFace, Room _relativeRoom)
        {
            RelativeNode = _relativeNode;
            RelativeRoom = _relativeRoom;
            transform.parent = RelativeRoom.transform;
            transform.rotation = /*_pointToFace*/ Quaternion.identity;

            //istanzio i contenitori
            InstantiateFloor();
            InstantiateEdges();
            InstantiateAngles();

            // istanzio la grafica
            InstantiateFloorElement();
            InstantiatePilllarElements();
            InstantiateWallElements();

            return this;
        }

        public bool CheckValidPosition(GridController _grid, out GridNode node)
        {
            node = _grid.GetSpecificGridNode(transform.position);
            if (node != null && node.RelativeCell == null)
                return true;

            return false;
        }

        public GridNode GetPositionOnGrid(GridController _grid)
        {
            return _grid.GetSpecificGridNode(transform.position);
        }

        public void SetRelativeNode(GridNode _relativeNode = null)
        {
            if(_relativeNode == null)
            {
                GridController grid = RelativeNode.RelativeGrid;
                RelativeNode = grid.GetSpecificGridNode(transform.position);
            }
            else
                RelativeNode = _relativeNode;
        }

        public void SetAdjacentCell()
        {
            foreach (Cell cellInRoom in RelativeRoom.CellsInRoom)
            {
                if(cellInRoom != this)
                {
                    if (Vector3.Distance(cellInRoom.transform.position, transform.position) <= RelativeNode.RelativeGrid.CellSize)
                    {
                        adjacentCells.Add(cellInRoom);
                    }
                }
            }
        }

        public void ShowInvalidPosition(bool _isInvalid)
        {
            //if (_isInvalid)
            //    foreach (MeshRenderer mesh in childrenMesh)
            //        mesh.material.color = Color.red;
            //else
            //    foreach (MeshRenderer mesh in childrenMesh)
            //        mesh.material.color = Color.white;
        }

        public List<GameObject> GetEdgesList()
        {
            Edges.RemoveAll(e => e == null);
            return Edges;
        }

        public List<GameObject> GetAnglesList()
        {
            Angles.RemoveAll(a => a == null);
            return Angles;
        }

        public List<Door> GetDoorsList()
        {
            Doors.RemoveAll(a => a == null);
            return Doors;
        }

        public void UpdateCellElements()
        {
            Doors = GetComponentsInChildren<Door>().ToList();
        }
        #endregion
    }
}