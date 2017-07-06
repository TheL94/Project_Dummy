﻿using DumbProject.Items;
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
        List<GridNode> fallingPoints = new List<GridNode>();
        GameObject floor;
        List<Edge> edges = new List<Edge>();
        List<GameObject> angles = new List<GameObject>();

        public IDroppable ActualDroppable;

        #region Cell Elements Instantiation
        /// <summary>
        /// Crea il contenitore del pavimento
        /// </summary>
        void InstantiateFloor()
        {
            floor = new GameObject("Floor");
            floor.transform.localPosition = transform.position;
            floor.transform.parent = transform;
            floor.transform.rotation = transform.rotation;
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
            edges.Add(newEdge);
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
            angles.Add(newAngle);

            newAngle = new GameObject("SE_Angle");
            newAngle.transform.localPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z - distance);
            newAngle.transform.parent = transform;
            angles.Add(newAngle);

            newAngle = new GameObject("NO_Angle");
            newAngle.transform.localPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z + distance);
            newAngle.transform.parent = transform;
            angles.Add(newAngle);

            newAngle = new GameObject("SO_Angle");
            newAngle.transform.localPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z - distance);
            newAngle.transform.parent = transform;
            angles.Add(newAngle);
        }

        /// <summary>
        /// Istanzia l'oggetto di grafica del pavimento
        /// </summary>
        void InstantiateFloorElement()
        {
            PlaceGameObj(GameManager.I.PoolMng.GetGameObject(ObjType.Floor), floor.transform);
        }

        /// <summary>
        /// Istanzia l'oggetto di grafica del pilastro
        /// </summary>
        void InstantiatePilllarElements()
        {
            foreach (GameObject angle in angles)
                PlaceGameObj(GameManager.I.PoolMng.GetGameObject(ObjType.Pillar), angle.transform);
        }

        /// <summary>
        /// Istanzia l'oggetto di grafica del muro
        /// </summary>
        void InstantiateWallElements()
        {
            foreach (Edge edge in edges)
            {
                if (edge.name == "RightEdge" || edge.name == "LeftEdge")
                {
                    edge.SetGraphic(GameManager.I.PoolMng.GetGameObject(ObjType.Wall), Quaternion.identity);
                }
                else if (edge.name == "UpEdge")
                {
                    edge.SetGraphic(GameManager.I.PoolMng.GetGameObject(ObjType.Wall), 
                        Quaternion.LookRotation(angles.Find(a => a.name == "NE_Angle").transform.position - edge.transform.position));
                }
                else if (edge.name == "DownEdge")
                {
                    edge.SetGraphic(GameManager.I.PoolMng.GetGameObject(ObjType.Wall),
                        Quaternion.LookRotation(angles.Find(a => a.name == "SE_Angle").transform.position - edge.transform.position));
                }
            }
        }

        void PlaceGameObj(GameObject _obj, Transform _transF)
        {
            _obj.transform.position = _transF.position;
            _obj.transform.rotation = _transF.rotation;
            _obj.transform.parent = _transF;
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
        public Cell PlaceCell(GridNode _relativeNode, Room _relativeRoom)
        {
            PlaceCell(_relativeNode, Quaternion.identity, _relativeRoom);
            return this;
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
        /// Funzione che collega la cella con le celle di altre stanze che sitrovano di fronte alle porte della cella
        /// </summary>
        public void LinkCellToOtherRoomsCells()
        {           
            foreach (Edge edge in edges)
            {
                if (edge.Type == EdgeType.Door)
                {
                    GridNode node = RelativeNode.RelativeGrid.GetSpecificGridNode(edge.GetNodeInFrontPosition());
                    if(node.RelativeCell != null)
                    adjacentCells.Add(node.RelativeCell);
                }
            }
            UpdateNodeLinks();
        }

        /// <summary>
        /// Funzione che collega la cella con le celle delle stessa stanza
        /// </summary>
        public void LinkCellToRelativeRoomCells()
        {
            foreach (GridNode adjacentNode in RelativeNode.AdjacentNodes)
            {
                if (adjacentNode.RelativeCell != null)
                {
                    adjacentCells.Add(adjacentNode.RelativeCell);
                }
            }
        }

        /// <summary>
        /// Funzione che riempe la lista di punti di caduta
        /// </summary>
        public void LinkDoorsToFallingPoint()
        {
            if (fallingPoints.Count > 0)
                fallingPoints.Clear();
            foreach (Edge edge in edges)
            {
                GridNode nodeInFront = RelativeNode.RelativeGrid.GetSpecificGridNode(edge.GetNodeInFrontPosition());
                if (edge.Type == EdgeType.Door && edge.CollidingEdge == null)
                {
                    if (nodeInFront != null && nodeInFront.RelativeCell == null && !fallingPoints.Contains(nodeInFront))
                        fallingPoints.Add(nodeInFront);
                }
                else if (edge.Type == EdgeType.Door && edge.CollidingEdge != null)
                {
                    if(fallingPoints.Contains(nodeInFront))
                        fallingPoints.Remove(nodeInFront);
                }
            }
            UpdateNodeLinks();
        }

        /// <summary>
        /// Funzione che aggiunge le celle colegate e i punti di caduta ai links del proprio nodo
        /// </summary>
        public void UpdateNodeLinks()
        {
            foreach (Cell adjacentCell in adjacentCells)
            {
                if(!RelativeNode.Links.Contains(adjacentCell.RelativeNode))
                    RelativeNode.Links.Add(adjacentCell.RelativeNode);
            }

            foreach (GridNode fallingPonint in fallingPoints)
            {
                if (!RelativeNode.Links.Contains(fallingPonint))
                    RelativeNode.Links.Add(fallingPonint);
            }
        } 

        /// <summary>
        /// Funzione che ritorna la lista degli edge di questa cella
        /// </summary>
        /// <returns></returns>
        public List<Edge> GetEdgesList()
        {
            edges.RemoveAll(e => e == null);
            return edges;
        }

        /// <summary>
        /// Funzione che ritorna la lista degli angles di questa cella
        /// </summary>
        /// <returns></returns>
        public List<GameObject> GetAnglesList()
        {
            angles.RemoveAll(a => a == null);
            return angles;
        }

        /// <summary>
        /// Funzione che ritorna la lista delle celle adiacenti
        /// </summary>
        /// <returns></returns>
        public List<Cell> GetAdjacentCellsList()
        {
            return adjacentCells;
        }

        /// <summary>
        /// Funzione che ritorna la lista dei punti di caduta
        /// </summary>
        /// <returns></returns>
        public List<GridNode> GetFallingPointsList()
        {
            return fallingPoints;
        }

        #region IChangeColor Interface

        public void ChangeFloorColor(Material _newMaterial)
        {
            floor.GetComponentInChildren<MeshRenderer>().material = _newMaterial;
        }

        #endregion
        #endregion

        private void OnDrawGizmos()
        {
            foreach (GridNode node in RelativeNode.Links)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(RelativeNode.WorldPosition, node.WorldPosition);
            }
        }
    }
}