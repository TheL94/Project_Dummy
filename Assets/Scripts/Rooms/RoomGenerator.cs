using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.UI;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class RoomGenerator : MonoBehaviour
    {
        public RoomData MainRoomTypesData;
        public RoomData RoomTypesData;
        [HideInInspector]
        public Room FirstRoom;

        RoomData MainRoomTypesInstances;
        RoomData RoomTypesInstances;
        List<SpawnsAssociation> SpawnsAssociations = new List<SpawnsAssociation>();

        //Variabile usata solo per differenziare il nome delle room 
        int numberOfRoomsCreated;

        public void Setup()
        {
            MainRoomTypesInstances = Instantiate(MainRoomTypesData);
            RoomTypesInstances = Instantiate(RoomTypesData);

            SetupSpawnsAssociations();

            FirstRoom = InstantiateFirstRoom(MainRoomTypesInstances);

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
                InstantiateRoom(RoomTypesInstances);
            }
        }

        public void ReleaseRoomSpawn(Room _room)
        {
            foreach (SpawnsAssociation association in SpawnsAssociations)
                if (association.Room == _room)
                {
                    association.Room = null;
                    association.GridSpawn.ClearNodesRelativeCell();
                }
        }

        Room InstantiateFirstRoom(RoomData _data)
        {
            GameObject newRoomObj = new GameObject();
            newRoomObj.transform.position = GameManager.I.MainGridCtrl.GetGridCenter().WorldPosition;
            Room mainRoom = newRoomObj.AddComponent<Room>();
            mainRoom.Setup(_data, GameManager.I.MainGridCtrl);
            mainRoom.name = "FirstRoom";
            mainRoom.AddNetNodesOnDoors();
            GameManager.I.DungeonMng.ParentRoom(mainRoom, ExplorationStatus.InExploration);

            GameManager.I.ItemManager.InstantiateItemInRoom(mainRoom);
            return mainRoom;
        }

        void InstantiateRoom(RoomData _data)
        {
            SpawnsAssociation association = GetFirstSpawnsAssociationAvailable();
            if (association != null)
            {
                GameObject newRoomObj = new GameObject("Room_" + numberOfRoomsCreated);
                newRoomObj.transform.position = association.GridSpawn.GetGridCenter().WorldPosition;
                newRoomObj.transform.parent = transform;
                Room room = newRoomObj.AddComponent<Room>();
                RoomMovement roomMovement = newRoomObj.AddComponent<RoomMovement>();
                association.Room = room;
                room.Setup(_data, association.GridSpawn, roomMovement);

                //Istanzia casualmente degli oggetti nella room
                float randNum = Random.Range(0f, 1f);

                if (randNum >= 0.1f)
                {
                    //for (int i = 0; i < Random.Range(0, 2); i++)
                    //{
                        GameManager.I.ItemManager.InstantiateItemInRoom(room);
                    //}
                }
                numberOfRoomsCreated++;
            }
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