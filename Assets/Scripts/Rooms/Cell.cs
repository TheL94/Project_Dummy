using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using System.Linq;

namespace DumbProject.Rooms.Cells
{
    public class Cell : MonoBehaviour
    {
        // TODO : caricamento prefab da spostare (in scriptable probabilmente), per adesso usare questo metodo per test.
        public GameObject FloorPrefab;
        public GameObject WallPrefab;
        public GameObject PillarPrefab;

        GridNode _relativeNode;
        public GridNode RelativeNode
        {
            get { return _relativeNode; }
            set
            {
                if(_relativeNode != null && _relativeNode.RelativeCell != null)
                    _relativeNode.RelativeCell = null;

                _relativeNode = value;
                if(_relativeNode != null)
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
            Floor.tag = "Floor";
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
            newEdge.tag = "Edge";
            Edges.Add(newEdge);

            newEdge = new GameObject("LeftEdge");
            newEdge.transform.localPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
            newEdge.transform.parent = transform;
            newRotation = ((transform.position - newEdge.transform.position) != Vector3.zero) ? Quaternion.LookRotation(transform.position - newEdge.transform.position) : Quaternion.identity;
            newEdge.transform.rotation = newRotation;
            newEdge.tag = "Edge";
            Edges.Add(newEdge);

            newEdge = new GameObject("UpEdge");
            newEdge.transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
            newEdge.transform.parent = transform;
            newRotation = ((transform.position - newEdge.transform.position) != Vector3.zero) ? Quaternion.LookRotation(transform.position - newEdge.transform.position) : Quaternion.identity;
            newEdge.transform.rotation = newRotation;
            newEdge.tag = "Edge";
            Edges.Add(newEdge);

            newEdge = new GameObject("DownEdge");
            newEdge.transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
            newEdge.transform.parent = transform;
            newRotation = ((transform.position - newEdge.transform.position) != Vector3.zero) ? Quaternion.LookRotation(transform.position - newEdge.transform.position) : Quaternion.identity;
            newEdge.transform.rotation = newRotation;
            newEdge.tag = "Edge";
            Edges.Add(newEdge);
        }

        void InstantiateAngles()
        {
            GameObject newAngle;
            int distance = (int)RelativeNode.RelativeGrid.CellSize / 2;

            newAngle = new GameObject("NE_Angle");
            newAngle.transform.localPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z + distance);
            newAngle.transform.parent = transform;
            newAngle.tag = "Angle";
            Angles.Add(newAngle);

            newAngle = new GameObject("SE_Angle");
            newAngle.transform.localPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z - distance);
            newAngle.transform.parent = transform;
            newAngle.tag = "Angle";
            Angles.Add(newAngle);

            newAngle = new GameObject("NO_Angle");
            newAngle.transform.localPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z + distance);
            newAngle.transform.parent = transform;
            newAngle.tag = "Angle";
            Angles.Add(newAngle);

            newAngle = new GameObject("SO_Angle");
            newAngle.transform.localPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z - distance);
            newAngle.transform.parent = transform;
            newAngle.tag = "Angle";
            Angles.Add(newAngle);
        }

        void InstantiateGraphics()
        {
            Instantiate(FloorPrefab, Floor.transform.position, Floor.transform.rotation, Floor.transform);

            foreach (GameObject angle in Angles)
            {
                Instantiate(PillarPrefab, angle.transform.position, Quaternion.identity, angle.transform);
            }

            foreach (GameObject edge in Edges)
            {
                if(edge.name == "RightEdge" || edge.name == "LeftEdge")
                    Instantiate(WallPrefab, edge.transform.position, Quaternion.identity, edge.transform);
                else if (edge.name == "UpEdge")
                    Instantiate(WallPrefab, edge.transform.position, Quaternion.LookRotation(Angles.Find(a => a.name == "NE_Angle").transform.position - edge.transform.position), edge.transform);
                else if (edge.name == "DownEdge")
                    Instantiate(WallPrefab, edge.transform.position, Quaternion.LookRotation(Angles.Find(a => a.name == "SE_Angle").transform.position - edge.transform.position), edge.transform);
            }
        }

        #region API
        public Cell PlaceCell(GridNode _relativeNode, Transform _pointToFace, Room _relativeRoom)
        {
            RelativeNode = _relativeNode;          
            relativeRoom = _relativeRoom;
            transform.parent = relativeRoom.transform;
            Quaternion newRotation = ((_pointToFace.position - transform.position) != Vector3.zero) ? Quaternion.LookRotation(_pointToFace.position - transform.position) : Quaternion.identity;
            transform.rotation = newRotation;
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
            transform.rotation = Quaternion.LookRotation(_pointToFace - transform.position);
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
            if(_isInvalid)
                foreach (MeshRenderer mesh in childrenMesh)
                    mesh.material.color = Color.red;
            else
                foreach (MeshRenderer mesh in childrenMesh)
                    mesh.material.color = Color.white;
        }
        #endregion
    }
}