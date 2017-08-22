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
        public RoomData ObjectiveRoomTypesData;
        public RoomData RoomTypesData;

        public float Distance;

        [HideInInspector]
        public Room FirstRoom;
        [HideInInspector]
        public Room ObjectiveRoom;

        RoomData MainRoomTypesInstance;
        RoomData ObjectiveRoomTypesInstance;
        RoomData RoomTypesInstance;
        List<SpawnsAssociation> SpawnsAssociations = new List<SpawnsAssociation>();

        //Variabile usata solo per differenziare il nome delle room 
        int numberOfRoomsCreated;

        public void Setup()
        {
            if(MainRoomTypesInstance == null)
                MainRoomTypesInstance = Instantiate(MainRoomTypesData);

            if (ObjectiveRoomTypesInstance == null)
                ObjectiveRoomTypesInstance = Instantiate(ObjectiveRoomTypesData);

            if (RoomTypesInstance == null)
                RoomTypesInstance = Instantiate(RoomTypesData);

            //if (SpawnsAssociations.Count == 0)
            SetupSpawnsAssociations();

            FirstRoom = InstantiateFirstRoom(MainRoomTypesInstance);

            ObjectiveRoom = InstantiateObjectiveRoom(ObjectiveRoomTypesInstance);

            for (int i = 0; i < SpawnsAssociations.Count; i++)
                InstantiateRoom(RoomTypesInstance);
        }

        public void Clean()
        {
            foreach (SpawnsAssociation association in SpawnsAssociations)
                association.Room.DestroyChildrenObject();

            for (int i = 0; i < SpawnsAssociations.Count; i++)
                if (SpawnsAssociations[i].Room != null)
                    Destroy(SpawnsAssociations[i].Room.gameObject);

            SpawnsAssociations.Clear();
        }

        public void CreateNewRoom()
        {
            if(GameManager.I.CurrentState == Flow.FlowState.Gameplay)
            {
                InstantiateRoom(RoomTypesInstance);
            }
        }

        public void ReleaseRoomSpawn(Room _room)
        {
            foreach (SpawnsAssociation association in SpawnsAssociations)
            {
                if (association.Room == _room)
                {
                    association.Room = null;
                    association.GridSpawn.ClearNodesRelativeCell();
                }
            }
        }

        #region Room Instatiation
        Room InstantiateFirstRoom(RoomData _data)
        {
            GameObject newRoomObj = new GameObject();
            Room mainRoom = newRoomObj.AddComponent<Room>();
            mainRoom.Setup(_data, GameManager.I.MainGridCtrl.GetGridCenter());
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
                newRoomObj.transform.parent = transform;
                Room room = newRoomObj.AddComponent<Room>();
                RoomMovement roomMovement = newRoomObj.AddComponent<RoomMovement>();
                association.Room = room;
                room.Setup(_data, association.GridSpawn.GetGridCenter(), roomMovement);

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

        Room InstantiateObjectiveRoom(RoomData _data)
        {
            GameObject newRoomObj = new GameObject();
            Room objectiveRoom = newRoomObj.AddComponent<Room>();
            objectiveRoom.Setup(_data, EvaluateGridPosition(GameManager.I.MainGridCtrl));
            objectiveRoom.name = "ObjectiveRoom";
            objectiveRoom.AddNetNodesOnDoors();
            GameManager.I.DungeonMng.ParentRoom(objectiveRoom, ExplorationStatus.Unavailable);

            //GameManager.I.ItemManager.InstantiateItemInRoom(objectiveRoom);
            return objectiveRoom;
        }

        GridNode EvaluateGridPosition(GridController _grid)
        {
            GridPosition gridRandomPosition;
            while (CalculateRandomGridPosition(_grid, out gridRandomPosition))
                continue;
            return _grid.GetSpecificGridNode(gridRandomPosition);
        }

        bool CalculateRandomGridPosition(GridController _grid, out GridPosition _randomPosition)
        {
            _randomPosition = new GridPosition(Random.Range(0, _grid.GridWidth), Random.Range(0, _grid.GridHeight));
            foreach (Cell cell in FirstRoom.CellsInRoom)
            {
                if (cell.RelativeNode.GridPosition == _randomPosition)
                    return true;
                else if(Vector3.Distance(cell.RelativeNode.WorldPosition, _grid.GetSpecificGridNode(_randomPosition).WorldPosition) < Distance)
                    return true;
            }
            return false;
        }

        #endregion
        #region Spawns Associations
        void SetupSpawnsAssociations()
        {
            for (int i = 0; i < GameManager.I.RoomPreviewCtrl.GridCtrls.Count || 
                i < GameManager.I.UIMng.GamePlayCtrl.LateralGUI.RoomPreviewController.UISpawns.Count; i++)
            {
                SpawnsAssociations.Add(new SpawnsAssociation(GameManager.I.RoomPreviewCtrl.GridCtrls[i],
                    GameManager.I.UIMng.GamePlayCtrl.LateralGUI.RoomPreviewController.UISpawns[i]));
            }
        }

        SpawnsAssociation GetFirstSpawnsAssociationAvailable()
        {
            foreach (SpawnsAssociation association in SpawnsAssociations)
                if (association.IsAvailable)
                    return association;
            return null;
        }
        #endregion
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