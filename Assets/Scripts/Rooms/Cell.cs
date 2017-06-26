﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        Room _relativeRoom;
        public Room RelativeRoom
        {
            get { return _relativeRoom; }
            private set { _relativeRoom = value; }
        }

        List<Cell> adjacentCells = new List<Cell>();

        GameObject Floor;
        List<Edge> Edges = new List<Edge>();
        List<GameObject> Angles = new List<GameObject>();

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
            GameObject newEdgeObj;

            int distance = (int)RelativeNode.RelativeGrid.CellSize / 2;

            newEdgeObj = new GameObject("RightEdge");
            newEdgeObj.transform.localPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
            SetupNewEdge(newEdgeObj);

            newEdgeObj = new GameObject("LeftEdge");
            newEdgeObj.transform.localPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
            SetupNewEdge(newEdgeObj);

            newEdgeObj = new GameObject("UpEdge");
            newEdgeObj.transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
            SetupNewEdge(newEdgeObj);

            newEdgeObj = new GameObject("DownEdge");
            newEdgeObj.transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
            SetupNewEdge(newEdgeObj);
        }

        /// <summary>
        /// Ruota e aggiunge i componenti necessari al nuovo edge; poi lo aggiunge alla lista di edge
        /// </summary>
        /// <param name="_newEdgeObj"></param>
        void SetupNewEdge(GameObject _newEdgeObj)
        {
            _newEdgeObj.transform.parent = transform;
            Quaternion newRotation = ((transform.position - _newEdgeObj.transform.position) != Vector3.zero) ? Quaternion.LookRotation(transform.position - _newEdgeObj.transform.position) : Quaternion.identity;
            _newEdgeObj.transform.rotation = newRotation;
            Edge newEdge = _newEdgeObj.AddComponent<Edge>();
            newEdge.Setup(this);
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
        /// Istanzia l'oggetto di grafica del pavimento
        /// </summary>
        void InstantiateFloorElement()
        {
            GameObject newObj = null;
            newObj = Instantiate(RelativeRoom.Data.RoomElements.FloorPrefab, Floor.transform.position, Floor.transform.rotation, Floor.transform);
        }

        /// <summary>
        /// Istanzia l'oggetto di grafica del pilastro
        /// </summary>
        void InstantiatePilllarElements()
        {
            GameObject newObj = null;
            foreach (GameObject angle in Angles)
            {
                newObj = Instantiate(RelativeRoom.Data.RoomElements.PillarPrefab, angle.transform.position, Quaternion.identity, angle.transform);
            }
        }

        /// <summary>
        /// Istanzia l'oggetto di grafica del muro
        /// </summary>
        void InstantiateWallElements()
        {
            GameObject newObj = null;
            foreach (Edge edge in Edges)
            {
                if (edge.name == "RightEdge" || edge.name == "LeftEdge")
                {
                    newObj = Instantiate(RelativeRoom.Data.RoomElements.WallPrefab, edge.transform.position, 
                        Quaternion.identity, edge.transform);
                }
                else if (edge.name == "UpEdge")
                {
                    newObj = Instantiate(RelativeRoom.Data.RoomElements.WallPrefab, edge.transform.position, 
                        Quaternion.LookRotation(Angles.Find(a => a.name == "NE_Angle").transform.position - edge.transform.position), edge.transform);
                }
                else if (edge.name == "DownEdge")
                {
                    newObj = Instantiate(RelativeRoom.Data.RoomElements.WallPrefab, edge.transform.position, 
                        Quaternion.LookRotation(Angles.Find(a => a.name == "SE_Angle").transform.position - edge.transform.position), edge.transform);
                }
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
        public Cell PlaceCell(GridNode _relativeNode, Transform _pointToFace, Room _relativeRoom)
        {
            Quaternion newRotation = ((_pointToFace.position - transform.position) != Vector3.zero) ? Quaternion.LookRotation(_pointToFace.position - transform.position) : Quaternion.identity;
            return PlaceCell(_relativeNode, newRotation, _relativeRoom);           
        }
        /// <summary>
        /// Funzione che continene tutte le istruzioni per creare una cella con tutta la sua grafica
        /// </summary>
        /// <param name="_relativeNode"></param>
        /// <param name="_pointToFace"></param>
        /// <param name="_relativeRoom"></param>
        /// <returns></returns>
        public Cell PlaceCell(GridNode _relativeNode, Vector3 _pointToFace, Room _relativeRoom)
        {
            Quaternion newRotation = Quaternion.LookRotation(_pointToFace - transform.position);
            return PlaceCell(_relativeNode, newRotation, _relativeRoom);
        }
        /// <summary>
        /// Funzione che continene tutte le istruzioni per creare una cella con tutta la sua grafica
        /// </summary>
        /// <param name="_relativeNode"></param>
        /// <param name="_pointToFace"></param>
        /// <param name="_relativeRoom"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Funzione che setta il nodo relativo della cella
        /// </summary>
        /// <param name="_relativeNode"></param>
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

        /// <summary>
        /// Funzione che mosta visivamente se il giocatore ha trascianto la stanza in una posizione invalida
        /// </summary>
        /// <param name="_isInvalid"></param>
        public void ShowInvalidPosition(bool _isInvalid)
        {
            //if (_isInvalid)
            //    foreach (MeshRenderer mesh in childrenMesh)
            //        mesh.material.color = Color.red;
            //else
            //    foreach (MeshRenderer mesh in childrenMesh)
            //        mesh.material.color = Color.white;
        }

        /// <summary>
        /// Funzione che ritorna la lista degli edge di questa cella
        /// </summary>
        /// <returns></returns>
        public List<Edge> GetEdgesList()
        {
            Edges.RemoveAll(e => e == null);
            return Edges;
        }

        /// <summary>
        /// Funzione che ritorna la lista degli angles di questa cella
        /// </summary>
        /// <returns></returns>
        public List<GameObject> GetAnglesList()
        {
            Angles.RemoveAll(a => a == null);
            return Angles;
        }

        /// <summary>
        /// Funzione che collega la cella con le celle di altre stanze che sitrovano di fronte alle porte della cella
        /// </summary>
        public void LinkCellToOtherRoomsCells()
        {           
            foreach (Edge edge in Edges)
            {
                if (edge.Type == EdgeType.Door && edge.CollidingEdge != null)
                    adjacentCells.Add(edge.CollidingEdge.RelativeCell);
            }
        }

        /// <summary>
        /// Funzione che collega la cella con le celle delle stessa stanza
        /// </summary>
        public void LinkCellToRelativeRoomCells()
        {
            foreach (GridNode node in RelativeNode.AdjacentNodes)
            {
                if (RelativeRoom.CellsInRoom.Contains(node.RelativeCell))
                    adjacentCells.Add(node.RelativeCell);
            }
        }
        #endregion
    }
}