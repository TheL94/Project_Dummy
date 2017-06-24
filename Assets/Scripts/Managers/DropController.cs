using System.Collections;
using System.Collections.Generic;
using DumbProject.Rooms;
using DumbProject.Rooms.Cells;
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
            if (CheckRoomPlacemnet(_room) && CheckDoorCollsions(_room))
                return true;
            else
                return false;
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
        /// Funzione che controlla che la stanza sia piazzata correttamanete sulla griglia
        /// </summary>
        /// <param name="_room"></param>
        /// <returns></returns>
        bool CheckRoomPlacemnet(Room _room)
        {
            bool isValidPosition = false;
            foreach (Cell cell in _room.CellsInRoom)
            {
                // Controllo della posizione della cella e della stanza
                GridNode node;
                if (!CheckCellValidPosition(cell, GameManager.I.MainGridCtrl, out node))
                    return false;

                foreach (GridNode adjacentNode in node.AdjacentNodes)
                {
                    if (adjacentNode.RelativeCell != null)
                    {
                        isValidPosition = true;
                    }
                }
            }
            return isValidPosition;
        }

        /// <summary>
        /// Funzione che controlla che le porte collidano in modo corretto
        /// </summary>
        /// <param name="_room"></param>
        /// <returns></returns>
        bool CheckDoorCollsions(Room _room)
        {
            bool isValidPosition = false;
            List<Edge> roomEdges = new List<Edge>();
            foreach (Cell cell in _room.CellsInRoom)
                roomEdges.AddRange(cell.GetEdgesList());

            // Controllo sulla posizione delle porte e dei muri
            foreach (Edge edge in roomEdges)
            {
                if (edge.Type == EdgeType.Door && edge.CollidingEdge != null)
                {
                    if (edge.CollidingEdge.Type == EdgeType.Door)
                        isValidPosition = true;
                    else
                        isValidPosition = false;
                }
                else if (edge.Type == EdgeType.Wall && edge.CollidingEdge != null)
                {
                    if (edge.CollidingEdge.Type == EdgeType.Door)
                        isValidPosition = false;
                    else
                    {
                        // Controllo che la stanza non sia collegata solo per muri
                        isValidPosition = false;
                        foreach (Edge eg in roomEdges)
                        {
                            if (eg.Type == EdgeType.Door && eg.CollidingEdge != null)
                            {
                                if (eg.CollidingEdge.Type == EdgeType.Door)
                                {
                                    isValidPosition = true;
                                }
                            }
                        }
                    }
                }
            }

            return isValidPosition;
        }
    }
}