﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Data;
using DumbProject.Grid;
using DumbProject.UI;

namespace DumbProject.Rooms
{
    public class RoomGenerator : MonoBehaviour
    {
        public List<RoomData> RoomTypesData = new List<RoomData>();

        List<RoomData> RoomTypesInstances = new List<RoomData>();

        List<SpawnsAssociation> SpawnsAssociations = new List<SpawnsAssociation>();

        public void Setup()
        {
            foreach (RoomData roomData in RoomTypesData)
            {
                RoomTypesInstances.Add(Instantiate(roomData));
            }

            SetupSpawnsAssociations();

            InstantiateFirstRoom(RoomTypesInstances[0]);
            CreateNewRoom();
            CreateNewRoom();
            CreateNewRoom();
        }

        public void CreateNewRoom()
        {
            foreach (RoomData roomData in RoomTypesInstances)
            {
                if (roomData.Shape == RoomShape.T_Shape)
                    InstantiateRoom(roomData);
            }
        }

        public void ReleaseRoomSpawn(Room _room)
        {
            foreach (SpawnsAssociation association in SpawnsAssociations)
                if (association.Room == _room)
                    association.Room = null;
        }

        void InstantiateFirstRoom(RoomData _data)
        {
            Room newRoom = Instantiate(_data.RoomPrefab, GameManager.I.MainGridCtrl.GetGridCenter().WorldPosition, Quaternion.identity);
            newRoom.PlaceMainRoom(_data, GameManager.I.MainGridCtrl);
            newRoom.name = "MainRoom";
        }

        void InstantiateRoom(RoomData _data)
        {
            SpawnsAssociation association = GetFirstSpawnsAssociationAvailable();
            if(association != null)
            {
                Room newRoom = Instantiate(_data.RoomPrefab, association.GridSpawn.GetGridCenter().WorldPosition, Quaternion.identity, transform);
                association.Room = newRoom;
                newRoom.Setup(_data, association.GridSpawn);
            }
        }

        void SetupSpawnsAssociations()
        {
            for (int i = 0; i < GameManager.I.RoomPreviewCtrl.GridCtrls.Count || i < GameManager.I.UIMng.roomPreviewController.UISpawns.Count; i++)
                SpawnsAssociations.Add(new SpawnsAssociation(GameManager.I.RoomPreviewCtrl.GridCtrls[i], GameManager.I.UIMng.roomPreviewController.UISpawns[i]));          
        }

        SpawnsAssociation GetFirstSpawnsAssociationAvailable()
        {
            foreach (SpawnsAssociation association in SpawnsAssociations)
                if (association.IsAvailable)
                    return association;
            return null;
        }
    }

    class SpawnsAssociation
    {
        public GridController GridSpawn;
        public UIRoomController UICtrl;
        Room _room;

        public Room Room
        {
            get { return _room; }
            set
            {
                _room = value;
                UICtrl.ActualRoom = _room;
                if (_room != null)
                    IsAvailable = false;
                else
                    IsAvailable = true;
            }
        }

        bool _isAvailable = true;
        public bool IsAvailable
        {
            get { return _isAvailable; }
            private set { _isAvailable = value; }
        }

        public SpawnsAssociation(GridController _gridSpawn, UIRoomController _uiCtrl)
        {
            GridSpawn = _gridSpawn;
            UICtrl = _uiCtrl;
        }
    }
}