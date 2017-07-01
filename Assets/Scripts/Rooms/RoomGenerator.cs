﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms.Data;
using DumbProject.Grid;
using DumbProject.UI;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class RoomGenerator : MonoBehaviour
    {
        public List<RoomData> RoomTypesData = new List<RoomData>();
        [HideInInspector]
        public Room FirstRoom;

        List<RoomData> RoomTypesInstances = new List<RoomData>();
        List<SpawnsAssociation> SpawnsAssociations = new List<SpawnsAssociation>();

        public void Setup()
        {
            foreach (RoomData roomData in RoomTypesData)
                RoomTypesInstances.Add(Instantiate(roomData));

            SetupSpawnsAssociations();

            FirstRoom = InstantiateFirstRoom();

            for (int i = 0; i < SpawnsAssociations.Count; i++)
                CreateNewRoom();
        }

        public void Clean()
        {
            foreach (SpawnsAssociation association in SpawnsAssociations)
                Destroy(association.Room.gameObject);

            SpawnsAssociations.Clear();
        }

        public void CreateNewRoom()
        {
            if(GameManager.I.FlowMng.CurrentState == Flow.FlowState.GameplayState)
            {
                RoomShape roomShape = GetRandomShape();
                foreach (RoomData roomData in RoomTypesInstances)
                {
                    if (roomData.Shape == roomShape)
                    {
                        InstantiateRoom(roomData);
                        break;
                    }
                }
            }
        }

        public void ReleaseRoomSpawn(Room _room)
        {
            foreach (SpawnsAssociation association in SpawnsAssociations)
                if (association.Room == _room)
                    association.Room = null;
        }

        Room InstantiateFirstRoom()
        {
            RoomData _data = null;
            Room mainRoom = null;
            GameObject newRoomObj = new GameObject();
            newRoomObj.transform.position = GameManager.I.MainGridCtrl.GetGridCenter().WorldPosition;

            RoomShape roomShape = GetRandomShape();
            foreach (RoomData roomData in RoomTypesInstances)
            {
                if (roomData.Shape == roomShape)
                {
                    _data = roomData;
                    mainRoom = AddRoomShapeComponent(_data, newRoomObj);
                    break;
                }
            }
            GameManager.I.DungeonMng.ParentRoom(mainRoom);
            mainRoom.Setup(_data, GameManager.I.MainGridCtrl);
            mainRoom.name = _data.Shape +  "_MainRoom";
            GameManager.I.DungeonMng.UpdateRoomConnections();
            return mainRoom;
        }

        void InstantiateRoom(RoomData _data)
        {
            SpawnsAssociation association = GetFirstSpawnsAssociationAvailable();
            if (association != null)
            {
                GameObject newRoomObj = new GameObject();
                newRoomObj.transform.position = association.GridSpawn.GetGridCenter().WorldPosition;
                newRoomObj.transform.parent = transform;

                Room room = AddRoomShapeComponent(_data, newRoomObj);

                RoomMovement roomMovement = newRoomObj.AddComponent<RoomMovement>();
                association.Room = room;
                room.Setup(_data, association.GridSpawn, roomMovement);

                //Istanzia casualmente degli oggetti nella room
                float randNum = Random.Range(0f, 1f);

                if (randNum >= 0.1f)
                {
                    //for (int i = 0; i < Random.Range(0, 2); i++)
                    //{
                        GameManager.I.ItemManager.InstantiateItemInRoom(room.ChooseFreeCell());
                    //}
                }
            }
        }

        RoomShape GetRandomShape()
        {
            int randomRoomShape = (int)Random.Range(0f, RoomTypesData.Count);
            return (RoomShape)randomRoomShape;
        }

        Room AddRoomShapeComponent(RoomData _data, GameObject _newRoomObj)
        {
            Room room = null;
            switch (_data.Shape)
            {
                case RoomShape.T_Shape:
                    room = _newRoomObj.AddComponent<TShapeRoom>();
                    room.name = "TShapeRoom";
                    break;
                case RoomShape.I_Shape:
                    room = _newRoomObj.AddComponent<IShapeRoom>();
                    room.name = "IShapeRoom";
                    break;
                case RoomShape.J_Shape:
                    room = _newRoomObj.AddComponent<JShapeRoom>();
                    room.name = "JShapeRoom";
                    break;
                case RoomShape.L_Shape:
                    room = _newRoomObj.AddComponent<LShapeRoom>();
                    room.name = "LShapeRoom";
                    break;
                case RoomShape.S_Shape:
                    room = _newRoomObj.AddComponent<SShapeRoom>();
                    room.name = "SShapeRoom";
                    break;
                case RoomShape.Z_Shape:
                    room = _newRoomObj.AddComponent<ZShapeRoom>();
                    room.name = "ZShapeRoom";
                    break;
                case RoomShape.O_Shape:
                    room = _newRoomObj.AddComponent<OShapeRoom>();
                    room.name = "OShapeRoom";
                    break;
            }
            return room;
        }

        void SetupSpawnsAssociations()
        {
            for (int i = 0; i < GameManager.I.RoomPreviewCtrl.GridCtrls.Count || i < GameManager.I.UIMng.GamePlayCtrl.GamePlayElements.RoomPreviewController.UISpawns.Count; i++)
                SpawnsAssociations.Add(new SpawnsAssociation(GameManager.I.RoomPreviewCtrl.GridCtrls[i], 
                    GameManager.I.UIMng.GamePlayCtrl.InventoryContainer.GetGamePlayElements().RoomPreviewController.UISpawns[i], 
                    GameManager.I.UIMng.GamePlayCtrl.VerticalInventoryContainer.GetGamePlayElements().RoomPreviewController.UISpawns[i]));          
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
        public UIRoomController VerticalUICtrl;

        Room _room;

        public Room Room
        {
            get { return _room; }
            set
            {
                _room = value;
                UICtrl.ActualRoom = _room;
                VerticalUICtrl.ActualRoom = _room;
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

        public SpawnsAssociation(GridController _gridSpawn, UIRoomController _uiCtrl, UIRoomController _verticalUiCtrl)
        {
            GridSpawn = _gridSpawn;
            UICtrl = _uiCtrl;
            VerticalUICtrl = _verticalUiCtrl;
        }
    }
}