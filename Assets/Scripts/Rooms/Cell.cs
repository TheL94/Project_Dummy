using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;

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

        [HideInInspector]
        public GameObject Floor;
        [HideInInspector]
        public List<GameObject> Edges = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> Angles = new List<GameObject>();

        MeshRenderer[] childrenMesh;
        Room relativeRoom;

        void InstantiateFloor()
        {
            Floor = new GameObject("Floor");
            Floor.transform.localPosition = transform.position;
            Floor.transform.parent = transform;
            Floor.transform.rotation = transform.rotation;
        }

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

        void InstantiateGraphics()
        {
            GameObject newObj;
            newObj = Instantiate(relativeRoom.Data.RoomElements.FloorPrefab, Floor.transform.position, Floor.transform.rotation, Floor.transform);
            newObj.tag = "Floor";

            foreach (GameObject angle in Angles)
            {
                newObj = Instantiate(relativeRoom.Data.RoomElements.PillarPrefab, angle.transform.position, Quaternion.identity, angle.transform);
                newObj.tag = "Angle";
            }

            foreach (GameObject edge in Edges)
            {
                if (edge.name == "RightEdge" || edge.name == "LeftEdge")
                {
                    newObj = Instantiate(relativeRoom.Data.RoomElements.WallPrefab, edge.transform.position, Quaternion.identity, edge.transform);
                    newObj.tag = "Edge";
                }
                else if (edge.name == "UpEdge")
                {
                    newObj = Instantiate(relativeRoom.Data.RoomElements.WallPrefab, edge.transform.position, Quaternion.LookRotation(Angles.Find(a => a.name == "NE_Angle").transform.position - edge.transform.position), edge.transform);
                    newObj.tag = "Edge";
                }
                else if (edge.name == "DownEdge")
                {
                    newObj = Instantiate(relativeRoom.Data.RoomElements.WallPrefab, edge.transform.position, Quaternion.LookRotation(Angles.Find(a => a.name == "SE_Angle").transform.position - edge.transform.position), edge.transform);
                    newObj.tag = "Edge";
                }
            }
        }

        #region API
        public Cell PlaceCell(GridNode _relativeNode, Transform _pointToFace, Room _relativeRoom)
        {
            RelativeNode = _relativeNode;
            relativeRoom = _relativeRoom;
            transform.parent = relativeRoom.transform;
            Quaternion newRotation = ((_pointToFace.position - transform.position) != Vector3.zero) ? Quaternion.LookRotation(_pointToFace.position - transform.position) : Quaternion.identity;
            transform.rotation = /*newRotation;*/ Quaternion.identity;
            InstantiateFloor();
            InstantiateEdges();
            InstantiateAngles();
            InstantiateGraphics();
            return this;
        }

        public Cell PlaceCell(GridNode _relativeNode, Vector3 _pointToFace, Room _relativeRoom)
        {
            RelativeNode = _relativeNode;
            relativeRoom = _relativeRoom;
            transform.parent = relativeRoom.transform;
            transform.rotation = /*Quaternion.LookRotation(_pointToFace - transform.position);*/ Quaternion.identity;
            InstantiateFloor();
            InstantiateEdges();
            InstantiateAngles();
            InstantiateGraphics();
            return this;
        }

        public void UpdateElements()
        {
            childrenMesh = GetComponentsInChildren<MeshRenderer>();
        }

        public bool CheckValidPosition(GridController _grid, out GridNode node)
        {
            node = _grid.GetSpecificGridNode(transform.position);
            if (node != null && node.RelativeCell == null)
                return true;

            return false;
        }

        //Usare dei Tween incatenati
        public GridNode GetMyPositionOnGrid(GridController _grid)
        {
            return _grid.GetSpecificGridNode(transform.position);
        }

        public void ResetRelativeNode()
        {
            GridController grid = RelativeNode.RelativeGrid;
            RelativeNode = grid.GetSpecificGridNode(transform.position);
        }

        public void SetRelativeNode(GridNode _relativeNode)
        {
            RelativeNode = _relativeNode;
        }

        public void ShowInvalidPosition(bool _isInvalid)
        {
            if (_isInvalid)
                foreach (MeshRenderer mesh in childrenMesh)
                    mesh.material.color = Color.red;
            else
                foreach (MeshRenderer mesh in childrenMesh)
                    mesh.material.color = Color.white;
        }
        #endregion
    }
}