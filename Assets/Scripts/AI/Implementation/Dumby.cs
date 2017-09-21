﻿using DumbProject.Rooms;
using Framework.AI;
using Framework.Pathfinding;
using UnityEngine;
using System.Collections.Generic;

namespace DumbProject.Generic
{
    public class Dumby : AI_Controller, IPathfinder
    {
        private Room _currentRoom;
        public Room CurrentRoom
        {
            get
            {
                if (_currentRoom == null)
                    _currentRoom = GameManager.I.MainGridCtrl.GetSpecificGridNode(transform.position).RelativeCell.RelativeRoom;
                return _currentRoom;
            }
            protected set { _currentRoom = value; }
        }

        #region IPathfinder
        public INetworkable Objective { get; set; }
        private INetworkable _currentNetworkable;
        public INetworkable CurrentNetworkable
        {
            get
            {
                if (_currentNetworkable == null)
                    _currentNetworkable = GameManager.I.MainGridCtrl.GetSpecificNetNode(transform.position);
                if (_currentNetworkable == null)
                    _currentNetworkable = GameManager.I.MainGridCtrl.GetSpecificGridNode(transform.position);
                return _currentNetworkable;
            }
            set
            {
                if (GameManager.I.MainGridCtrl.GetSpecificNetNode(value.spacePosition) == null)
                {
                    Cell currentCell = GameManager.I.MainGridCtrl.GetSpecificGridNode(value.spacePosition).RelativeCell;
                    if(currentCell != null)
                        CurrentRoom = GameManager.I.MainGridCtrl.GetSpecificGridNode(value.spacePosition).RelativeCell.RelativeRoom;
                }
                _currentNetworkable = value;
            }
        }

        public Path Path { get; set; }
        #endregion
    }
}
