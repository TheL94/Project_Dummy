﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DumbProject.Grid;
using DumbProject.Rooms.Data;
using DumbProject.Rooms.Cells;
using DumbProject.Rooms.Cells.Doors;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    /// <summary>
    /// Classe astatta padre di ogni tipo di stanza
    /// </summary>
    public abstract class Room : MonoBehaviour
    {
        [HideInInspector]
        public List<Cell> CellsInRoom;
        [HideInInspector]
        public List<Door> DoorsInRoom = new List<Door>();
        [HideInInspector]
        public RoomMovement RoomMovment;
        [HideInInspector]
        public DropController DropController;
        [HideInInspector]
        public RoomData Data;

        Vector3 _initialPosition;
        Tweener rotationTween;
        public Vector3 InitialPosition
        {
            get { return _initialPosition; }
            private set { _initialPosition = value; }
        }

        bool canRotate = true;

        #region API
        /// <summary>
        /// Setup della room nella UI
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        /// <param name="_roomMovment"></param>
        public void Setup(RoomData _data, GridController _grid, RoomMovement _roomMovment)
        {
            RoomMovment = _roomMovment;
            RoomMovment.Init(this);
            InitialPosition = transform.position;
            Setup(_data, _grid);
        }

        /// <summary>
        /// Setup della room come main room
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        public void Setup(RoomData _data, GridController _grid)
        {
            Data = _data;
            PlaceCells(_data, _grid);
            LinkCells();
            TrimCellWalls();
            PlaceDoors();
            TrimCellPillars();
            UpdateCellsElements();
        }
        
        /// <summary>
        /// Funzione che contiene le azioni da eseguire quando la posizione in cui si vuole metere la stanza è valida e la stanza viene piazzata
        /// </summary>
        public void PlaceAction()
        {
            foreach (Cell cell in CellsInRoom)
                cell.SetRelativeNode(cell.GetPositionOnGrid(GameManager.I.MainGridCtrl));

            GameManager.I.DungeonMng.ParentRoom(this);
            Destroy(RoomMovment);
        }

        /// <summary>
        /// funzione che fa ruotare la stanza attorno al suo centro in senso orario sulla griglia 
        /// </summary>
        public void RotateClockwise()
        {
            if (canRotate)
            {
                canRotate = false;
                rotationTween = transform.DORotate(transform.up * 90f, 0.5f, RotateMode.LocalAxisAdd).OnComplete(() => 
                {
                    canRotate = true;
                    foreach (Cell cell in CellsInRoom)
                        cell.SetRelativeNode();
                });
            }
        }

        #region Object Placement
        /// <summary>
        /// Ritorna una cella random dove istanziare l'item, se la cella è occupata, ne cerca una libera.
        /// </summary>
        /// <returns>cella libera</returns>
        public Cell ChooseFreeCell()
        {
            int tempNum = Random.Range(0, CellsInRoom.Count);
            if (CellsInRoom[tempNum].IsFree)
                return CellsInRoom[tempNum];
            else
            {
                for (int i = 0; i < CellsInRoom.Count; i++)
                {
                    if (CellsInRoom[i].IsFree)
                        return CellsInRoom[i];
                }
            }
            return null;
        }
        #endregion
        #endregion

        /// <summary>
        /// Funzione che piazza la stanza sulla griglia nella UI
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_grid"></param>
        protected abstract void PlaceCells(RoomData _data, GridController _grid);

        #region Cell Managment
        /// <summary>
        /// Funzione che rimuove i muri se sono nella stessa posizione
        /// </summary>
        void TrimCellWalls()
        {
            List<GameObject> itemsToBeDestroyed = new List<GameObject>();
            List<GameObject> listOfWalls = GetListOfWalls();

            foreach (GameObject wall1 in listOfWalls)
            {
                foreach (GameObject wall2 in listOfWalls)
                {
                    if(wall1 != wall2 && !itemsToBeDestroyed.Contains(wall1) && !itemsToBeDestroyed.Contains(wall2))
                    {
                        if(Vector3.Distance(wall1.transform.position, wall2.transform.position) <= Data.PenetrationOffset)
                        {
                            itemsToBeDestroyed.Add(wall1);
                            itemsToBeDestroyed.Add(wall2);
                        }
                    }
                }
            }

            foreach (Cell cell in CellsInRoom)
            {
                List<GameObject> list = cell.GetEdgesList();
                foreach (GameObject wallToRemove in itemsToBeDestroyed)
                {
                    if (list.Contains(wallToRemove))
                    {
                        list.Remove(wallToRemove);
                        Destroy(wallToRemove);
                    }
                }
            }
        }

        /// <summary>
        /// Funzione che rimuove i pilastri se sono nella stessa posizione
        /// </summary>
        void TrimCellPillars()
        {
            List<GameObject> itemsToBeDestroyed = new List<GameObject>();
            List<GameObject> listOfPillars = GetListOfPillars();

            foreach (GameObject pillar1 in listOfPillars)
            {
                foreach (GameObject pillar2 in listOfPillars)
                {
                    if (pillar1 != pillar2 && !itemsToBeDestroyed.Contains(pillar1) && !itemsToBeDestroyed.Contains(pillar2))
                    {
                        if (pillar1.transform.position == pillar2.transform.position)
                        {
                            itemsToBeDestroyed.Add(pillar2);
                        }
                    }
                }
            }

            foreach (Cell cell in CellsInRoom)
            {
                List<GameObject> list = cell.GetAnglesList();
                foreach (GameObject pillarToRemove in itemsToBeDestroyed)
                {
                    if (list.Contains(pillarToRemove))
                    {
                        list.Remove(pillarToRemove);
                        Destroy(pillarToRemove);
                    }
                }
            }
        }

        /// <summary>
        /// Funzione che sostiuisce a random alcuni muri con delle porte
        /// </summary>
        void PlaceDoors()
        {
            int numberOfDoors = Random.Range(1, CellsInRoom.Count + 1);
            List<GameObject> listOfWalls = GetListOfWalls();
            while (numberOfDoors > 0)
            {
                int randomIndex = Random.Range(0, listOfWalls.Count);
                DoorsInRoom.Add(ReplaceWallWithArch(listOfWalls[randomIndex]));
                listOfWalls.Remove(listOfWalls[randomIndex]);
                numberOfDoors--;
            }
        }

        /// <summary>
        /// Funzione che sostituisce la grafica del muro con la grafica porta
        /// </summary>
        /// <param name="_edge"></param>
        Door ReplaceWallWithArch(GameObject _edge)
        {
            GameObject newObj = null;
            Cell parentCell = null;
            Destroy(_edge.GetComponentInChildren<MeshRenderer>().gameObject);
            parentCell = _edge.transform.parent.GetComponent<Cell>();
            if (_edge.name == "RightEdge")
            {
                newObj = Instantiate(Data.RoomElements.ArchPrefab, _edge.transform.position, Quaternion.identity, _edge.transform);
                _edge.name = "RightDoor";
            }
            else if (_edge.name == "LeftEdge")
            {
                newObj = Instantiate(Data.RoomElements.ArchPrefab, _edge.transform.position, Quaternion.identity, _edge.transform);
                _edge.name = "LeftDoor";
            }
            else if (_edge.name == "UpEdge")
            {
                newObj = Instantiate(Data.RoomElements.ArchPrefab, _edge.transform.position, 
                    Quaternion.LookRotation(parentCell.GetAnglesList().Find(a => a.name == "NE_Angle").transform.position - _edge.transform.position), _edge.transform);
                _edge.name = "UpDoor";
            }
            else if (_edge.name == "DownEdge")
            {
                newObj = Instantiate(Data.RoomElements.ArchPrefab, _edge.transform.position, 
                    Quaternion.LookRotation(parentCell.GetAnglesList().Find(a => a.name == "SE_Angle").transform.position - _edge.transform.position), _edge.transform);
                _edge.name = "DownDoor";
            }
            newObj.tag = "Door";

            Door door = _edge.AddComponent<Door>();
            door.Setup(parentCell);
            return door;
        }

        /// <summary>
        /// Funzione che collaga fr di loro le celle adiacenti
        /// </summary>
        void LinkCells()
        {
            foreach (Cell cell in CellsInRoom)
                cell.SetAdjacentCell();
        }

        /// <summary>
        /// Ritorna la lista dei muri contenuti in tutte le celle
        /// </summary>
        /// <returns></returns>
        public List<GameObject> GetListOfWalls()
        {
            List<GameObject> listOfWalls = new List<GameObject>();
            foreach (Cell cell in CellsInRoom)
            {
                foreach (GameObject wall in cell.GetEdgesList())
                {
                    listOfWalls.Add(wall);
                }
            }
            return listOfWalls;
        }

        /// <summary>
        /// Ritorna la lista dei pilastri contenuti in tutte le celle
        /// </summary>
        /// <returns></returns>
        List<GameObject> GetListOfPillars()
        {
            List<GameObject> listOfPillars = new List<GameObject>();
            foreach (Cell cell in CellsInRoom)
            {
                foreach (GameObject wall in cell.GetAnglesList())
                {
                    listOfPillars.Add(wall);
                }
            }
            return listOfPillars;
        }

        void UpdateCellsElements()
        {
            foreach (Cell cell in CellsInRoom)
            {
                cell.UpdateCellElements();
            }
        }
        #endregion
    }                                      
       
    /// <summary>
    /// Tipi di stanze
    /// </summary>
    public enum RoomShape
    {
        T_Shape = 0,
        I_Shape = 1,
        J_Shape = 2,
        L_Shape = 3,
        S_Shape = 4,
        Z_Shape = 5,
        O_Shape = 6,
    }
}