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
        // varibile usata per la distanza minima tra stanza di partenza e stanza obbiettivo
        public float Distance;

        RoomData MainRoomDataInstance;
        RoomData ObjectiveRoomDataInstance;
        RoomData RoomDataInstance;
        List<SpawnsAssociation> SpawnsAssociations = new List<SpawnsAssociation>();

        //Variabile usata solo per differenziare il nome delle room 
        int numberOfRoomsCreated;

        public void Init(RoomData _mainRoomDataInstance, RoomData _objectiveRoomDataInstance, RoomData _roomDataInstance)
        {
            MainRoomDataInstance = _mainRoomDataInstance;
            ObjectiveRoomDataInstance = _objectiveRoomDataInstance;
            RoomDataInstance = _roomDataInstance;
        }

        public void Setup()
        {
            SetupSpawnsAssociations();

            GameManager.I.DungeonMng.FirstRoom = InstantiateFirstRoom(MainRoomDataInstance);

            GameManager.I.DungeonMng.ObjectiveRoom = InstantiateObjectiveRoom(ObjectiveRoomDataInstance);

            for (int i = 0; i < SpawnsAssociations.Count; i++)
                InstantiateRoom(RoomDataInstance);
        }

        public void ResetGraphics()
        {
            foreach (SpawnsAssociation association in SpawnsAssociations)
                association.Room.ResetChildrenGraphics();
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
                InstantiateRoom(RoomDataInstance);
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
                roomMovement.Init(room);

                //Istanzia casualmente degli oggetti nella room
                float randNum = Random.Range(0f, 1f);

                if (randNum >= 0.1f)
                    GameManager.I.GDR_Element_Mng.AddGDR_ElementInRoom(room);

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

            newRoomObj.AddComponent<IndicatorController>().Init();

            return objectiveRoom;
        }

        GridNode EvaluateGridPosition(GridController _grid)
        {
            GridPosition gridRandomPosition;
            while (CalculateRandomGridPosition(_grid, out gridRandomPosition, GameManager.I.DungeonMng.FirstRoom))
                continue;
            return _grid.GetSpecificGridNode(gridRandomPosition);
        }

        bool CalculateRandomGridPosition(GridController _grid, out GridPosition _randomPosition, Room _firstRoom)
        {
            _randomPosition = new GridPosition(Random.Range(0, _grid.GridWidth), Random.Range(0, _grid.GridHeight));
            foreach (Cell cell in _firstRoom.CellsInRoom)
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
                i < GameManager.I.UIMng.GamePlayCtrl.RoomPanelContainer.UISpawns.Count; i++)
            {
                SpawnsAssociations.Add(new SpawnsAssociation(GameManager.I.RoomPreviewCtrl.GridCtrls[i],
                    GameManager.I.UIMng.GamePlayCtrl.RoomPanelContainer.UISpawns[i]));
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