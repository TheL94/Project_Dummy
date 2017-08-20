using Framework.Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class Cell : MonoBehaviour, IChangeColor
    {
        GridNode _relativeNode;
        public GridNode RelativeNode
        {
            get { return _relativeNode; }
            set
            {
                if (_relativeNode != null && RelativeNode.RelativeCell != null)
                    RelativeNode.RelativeCell = null;

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

        Floor _floor;
        public Floor Floor { get { return _floor; } set { _floor = value; } }

        List<Door> _doors = new List<Door>();
        public List<Door> Doors { get { return _doors; } private set { _doors = value; } }

        List<Edge> _edges = new List<Edge>();
        public List<Edge> Edges { get { return _edges; } private set { _edges = value; } }

        List<Angle> _angles = new List<Angle>();
        public List<Angle> Angles { get { return _angles; } private set { _angles = value; } }

        IInteractable _actualInteractable;
        public IInteractable ActualInteractable
        {
            get { return _actualInteractable; }
            set
            {
                // TODO : posizionare bene l'oggetto
                _actualInteractable = value;
                _actualInteractable.Transf.parent = RelativeRoom.transform;
                _actualInteractable.Transf.position = transform.position;
                _actualInteractable.Transf.rotation = transform.rotation;
            }
        }

        #region Cell Elements Placing
        /// <summary>
        /// Crea il contenitore del pavimento
        /// </summary>
        void InstantiateFloor()
        {
            GameObject floorObj = new GameObject("Floor");
            Floor = floorObj.AddComponent<Floor>();
            Floor.Setup(this);
            Floor.transform.localPosition = transform.position;
            Floor.transform.parent = transform;
            Floor.transform.rotation = transform.rotation;
        }

        /// <summary>
        /// Crea i contenitori dei muri
        /// </summary>
        void InstantiateEdges()
        {
            GameObject newEdgeObj;
            float distance = RelativeNode.RelativeGrid.CellSize / 2;

            newEdgeObj = new GameObject("RightEdge");
            SetupNewEdge(newEdgeObj, new Vector3(transform.position.x + distance, transform.position.y, transform.position.z));

            newEdgeObj = new GameObject("LeftEdge");
            SetupNewEdge(newEdgeObj, new Vector3(transform.position.x - distance, transform.position.y, transform.position.z));

            newEdgeObj = new GameObject("UpEdge");
            SetupNewEdge(newEdgeObj, new Vector3(transform.position.x, transform.position.y, transform.position.z + distance));

            newEdgeObj = new GameObject("DownEdge");
            SetupNewEdge(newEdgeObj, new Vector3(transform.position.x, transform.position.y, transform.position.z - distance));
        }

        /// <summary>
        /// Ruota e aggiunge i componenti necessari al nuovo edge; poi lo aggiunge alla lista di edge
        /// </summary>
        /// <param name="_newEdgeObj"></param>
        void SetupNewEdge(GameObject _newEdgeObj, Vector3 _position)
        {
            _newEdgeObj.transform.localPosition = _position;
            _newEdgeObj.transform.parent = transform;
            //Quaternion newRotation = ((transform.position - _newEdgeObj.transform.position) != Vector3.zero) ? 
            //    Quaternion.LookRotation(transform.position - _newEdgeObj.transform.position) : Quaternion.identity;
            //_newEdgeObj.transform.rotation = newRotation;
            Edge newEdge = _newEdgeObj.AddComponent<Edge>();
            newEdge.Setup(this);
            Edges.Add(newEdge);
        }

        /// <summary>
        /// Crea il contenitore dei pilastri
        /// </summary>
        void InstantiateAngles()
        {
            GameObject newAngleObj;
            Angle newAngle;
            float distance = RelativeNode.RelativeGrid.CellSize / 2;

            newAngleObj = new GameObject("NE_Angle");
            newAngle = newAngleObj.AddComponent<Angle>();
            newAngle.Setup(this);
            newAngleObj.transform.localPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z + distance);
            newAngleObj.transform.parent = transform;
            Angles.Add(newAngle);

            newAngleObj = new GameObject("SE_Angle");
            newAngle = newAngleObj.AddComponent<Angle>();
            newAngle.Setup(this);
            newAngleObj.transform.localPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z - distance);
            newAngleObj.transform.parent = transform;
            Angles.Add(newAngle);

            newAngleObj = new GameObject("NO_Angle");
            newAngle = newAngleObj.AddComponent<Angle>();
            newAngle.Setup(this);
            newAngleObj.transform.localPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z + distance);
            newAngleObj.transform.parent = transform;
            Angles.Add(newAngle);

            newAngleObj = new GameObject("SO_Angle");
            newAngle = newAngleObj.AddComponent<Angle>();
            newAngle.Setup(this);
            newAngleObj.transform.localPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z - distance);
            newAngleObj.transform.parent = transform;
            Angles.Add(newAngle);
        }

        /// <summary>
        /// Istanzia l'oggetto di grafica del pavimento
        /// </summary>
        void InstantiateFloorElement()
        {
            Floor.SetGraphic(GameManager.I.PoolMng.GetGameObject(ObjType.Floor), Quaternion.identity);
        }

        /// <summary>
        /// Istanzia l'oggetto di grafica del pilastro
        /// </summary>
        void InstantiatePilllarElements()
        {
            foreach (Angle angle in Angles)
                angle.SetGraphic(GameManager.I.PoolMng.GetGameObject(ObjType.Pillar), Quaternion.identity);
        }

        /// <summary>
        /// Istanzia l'oggetto di grafica del muro
        /// </summary>
        void InstantiateWallElements()
        {
            foreach (Edge edge in Edges)
            {
                edge.SetGraphic(GameManager.I.PoolMng.GetGameObject(ObjType.Wall), Quaternion.LookRotation(transform.position - edge.transform.position));
            }
        }
        #endregion

        #region API
        /// <summary>
        /// Funzione che continene tutte le istruzioni per creare una cella con tutta la sua grafica
        /// </summary>
        /// <param name="_relativeNode"></param>
        /// <param name="_pointToFace"></param>
        /// <param name="_relativeRoom"></param>
        /// <returns></returns>
        public Cell PlaceCell(GridNode _relativeNode, Room _relativeRoom)
        {
            RelativeNode = _relativeNode;
            RelativeRoom = _relativeRoom;
            transform.parent = RelativeRoom.transform;

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

        /// <summary>
        /// Funzione che sostituisce la grafica del muro con la grafica porta
        /// </summary>
        /// <param name="_edge"></param>
        public bool ReplaceEdgeWithDoor(Edge _edge)
        {
            if (_edge.GetType() == typeof(Door))
                return false;

            GameObject edgeObj = _edge.gameObject;
            _edge.DisableAndDestroyObject(true);
            Door newDoor = edgeObj.AddComponent<Door>();

            newDoor.Setup(this);
            newDoor.SetGraphic(GameManager.I.PoolMng.GetGameObject(ObjType.Arch), Quaternion.LookRotation(transform.position - newDoor.transform.position));

            if (_edge.name == "RightEdge")
                newDoor.name = "RightDoor";
            else if (_edge.name == "LeftEdge")
                newDoor.name = "LeftDoor";
            else if (_edge.name == "UpEdge")
                newDoor.name = "UpDoor";
            else if (_edge.name == "DownEdge")
                newDoor.name = "DownDoor";

            Doors.Add(newDoor);
            return true;
        }

        /// <summary>
        /// Funzione che aggiorna i links del nodo relativo della cella
        /// </summary>
        public void UpdateRelativeNodeLinks()
        {
            // Aggiungo ai links del nodo relativo le celle delle stanza relativa adiacenti
            foreach (GridNode adjacentNode in RelativeNode.AdjacentNodes)
                if (adjacentNode.RelativeCell != null && adjacentNode.RelativeCell.RelativeRoom == RelativeRoom)
                    RelativeNode.AddLinks(new List<INetworkable>() { adjacentNode });

            // Aggiungo ai links del nodo relativo le mie porte
            foreach (Door door in Doors)
            {
                door.UpdateLinks();
            }
        }

        #region IChangeColor Interface

        public void ChangeFloorColor(Material _newMaterial)
        {
            Floor.GetComponentInChildren<MeshRenderer>().material = _newMaterial;
        }

        #endregion
        #endregion

        private void OnDrawGizmos()
        {
            if (RelativeNode == null)
                return;
            Gizmos.color = Color.green;
            foreach (INetworkable link in RelativeNode.Links)
            {
                if (link != null)
                    Gizmos.DrawLine(RelativeNode.WorldPosition + new Vector3(0f, .5f, 0f), link.spacePosition + new Vector3(0f, .5f, 0f));
            }
        }
    }
}