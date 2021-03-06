﻿using Framework.Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Generic;
using DumbProject.GDR_System;

namespace DumbProject.Rooms
{
    public class Cell : MonoBehaviour
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
                _actualInteractable = value;
                _actualInteractable.Transf.parent = transform;
                _actualInteractable.Transf.position = transform.position;
            }
        }

        // variabile usata per posizionare gli elementi della cella
        float Distance { get { return RelativeNode.RelativeGrid.CellSize / 2; } }

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

            newEdgeObj = new GameObject("RightEdge");
            SetupNewEdge(newEdgeObj, new Vector3(transform.position.x + Distance, transform.position.y, transform.position.z));

            newEdgeObj = new GameObject("LeftEdge");
            SetupNewEdge(newEdgeObj, new Vector3(transform.position.x - Distance, transform.position.y, transform.position.z));

            newEdgeObj = new GameObject("UpEdge");
            SetupNewEdge(newEdgeObj, new Vector3(transform.position.x, transform.position.y, transform.position.z + Distance));

            newEdgeObj = new GameObject("DownEdge");
            SetupNewEdge(newEdgeObj, new Vector3(transform.position.x, transform.position.y, transform.position.z - Distance));
        }

        /// <summary>
        /// Ruota e aggiunge i componenti necessari al nuovo edge; poi lo aggiunge alla lista di edge
        /// </summary>
        /// <param name="_newEdgeObj"></param>
        void SetupNewEdge(GameObject _newEdgeObj, Vector3 _position)
        {
            _newEdgeObj.transform.localPosition = _position;
            _newEdgeObj.transform.parent = transform;
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

            newAngleObj = new GameObject("NE_Angle");
            newAngle = newAngleObj.AddComponent<Angle>();
            newAngle.Setup(this);
            newAngleObj.transform.localPosition = new Vector3(transform.position.x + Distance, transform.position.y, transform.position.z + Distance);
            newAngleObj.transform.parent = transform;
            Angles.Add(newAngle);

            newAngleObj = new GameObject("SE_Angle");
            newAngle = newAngleObj.AddComponent<Angle>();
            newAngle.Setup(this);
            newAngleObj.transform.localPosition = new Vector3(transform.position.x + Distance, transform.position.y, transform.position.z - Distance);
            newAngleObj.transform.parent = transform;
            Angles.Add(newAngle);

            newAngleObj = new GameObject("NO_Angle");
            newAngle = newAngleObj.AddComponent<Angle>();
            newAngle.Setup(this);
            newAngleObj.transform.localPosition = new Vector3(transform.position.x - Distance, transform.position.y, transform.position.z + Distance);
            newAngleObj.transform.parent = transform;
            Angles.Add(newAngle);

            newAngleObj = new GameObject("SO_Angle");
            newAngle = newAngleObj.AddComponent<Angle>();
            newAngle.Setup(this);
            newAngleObj.transform.localPosition = new Vector3(transform.position.x - Distance, transform.position.y, transform.position.z - Distance);
            newAngleObj.transform.parent = transform;
            Angles.Add(newAngle);
        }

        #region Graphic Instantiation
        /// <summary>
        /// Istanzia l'oggetto di grafica del pavimento
        /// </summary>
        void InstantiateFloorElement()
        {
            Floor.SetGraphicElement(GameManager.I.PoolMng.GetGameObject("Floor"), Quaternion.identity);
            Floor.SetFillerGraphic(GameManager.I.PoolMng.GetGameObject("Rock"), Floor.transform.position, Floor.transform.rotation);
        }

        /// <summary>
        /// Istanzia l'oggetto di grafica del pilastro
        /// </summary>
        void InstantiatePilllarElements()
        {
            foreach (Angle angle in Angles)
                angle.SetGraphicElement(GameManager.I.PoolMng.GetGameObject("Column"), Quaternion.identity);
        }

        /// <summary>
        /// Istanzia l'oggetto di grafica del muro
        /// </summary>
        void InstantiateWallElements()
        {
            foreach (Edge edge in Edges)
            {
                edge.SetGraphicElement(GameManager.I.PoolMng.GetGameObject("Wall"), Quaternion.LookRotation(transform.position - edge.transform.position));
            }
        }
        #endregion
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
            _edge.DisableAllGraphics();
            _edge.DestroyObject(true);
            Door newDoor = edgeObj.AddComponent<Door>();

            newDoor.SetGraphicElement(GameManager.I.PoolMng.GetGameObject("WallDoor"), Quaternion.LookRotation(transform.position - newDoor.transform.position));
            newDoor.Setup(this);

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
        #endregion
    }
}