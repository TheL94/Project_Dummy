using System.Collections;
using System.Collections.Generic;
using DumbProject.Rooms;
using DumbProject.Rooms.Cells;
using DumbProject.Rooms.Cells.Doors;
using DumbProject.Grid;

namespace DumbProject.Generic
{
    public class DropController
    {
        public DropController() { }

        /// <summary>
        /// Funzione che controlla che la posizione di ogni cella della stanza sia valida
        /// </summary>
        public bool CheckRoomValidPosition(Room _room)
        {
            bool isValidPosition = false;
            foreach (Cell cell in _room.CellsInRoom)
            {
                GridNode node;
                if (!CheckCellValidPosition(cell, GameManager.I.MainGridCtrl, out node))
                    return false;

                foreach (GridNode adjacentNode in node.AdjacentNodes)
                {
                    if (adjacentNode.RelativeCell != null)
                    {
                        foreach (Door door in _room.DoorsInRoom)
                        {
                            if (door.CheckCollisionWithOtherDoors(adjacentNode.RelativeCell))
                                isValidPosition = true;
                            else
                                isValidPosition = false;
                        }
                    }
                }
            }
            return isValidPosition;
        }

        /// <summary>
        /// Funzione che controlla che la posizione della cella sia valida
        /// </summary>
        /// <param name="_cell"></param>
        /// <param name="_grid"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool CheckCellValidPosition(Cell _cell, GridController _grid, out GridNode node)
        {
            node = _grid.GetSpecificGridNode(_cell.transform.position);
            if (node != null && node.RelativeCell == null)
                return true;

            return false;
        }

        /// <summary>
        /// Funzione che controlla che nessuna delle porte delle stanze già piazzate venga chiusa con un muro
        /// </summary>
        /// <returns></returns>
        public bool CheckAdjacentRoomDoorRules()
        {
            bool areRulesValid = false;
            foreach (Room room in GameManager.I.DungeonMng.RoomInDungeon)
            {
                foreach (Door door in room.DoorsInRoom)
                {
                    if (door.CheckCollisionWithOtherWalls(room))
                        areRulesValid = true;
                }
            }
            return areRulesValid;
        }
    }
}