using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;

namespace DumbProject.Generic
{
    public class DataManager : MonoBehaviour
    {
        public RoomData MainRoomData;
        public RoomData ObjectiveRoomData;
        public RoomData RoomData;
        public List<RoomGraphicComponent> RoomGraphicComponentDatas = new List<RoomGraphicComponent>();

        public RoomData MainRoomDataInst;
        public RoomData ObjectiveRoomDataInst;
        public RoomData RoomDataInst;
        public List<RoomGraphicComponent> RoomGraphicComponentDatasInst = new List<RoomGraphicComponent>();

        public void InitData()
        {
            MainRoomDataInst = Instantiate(MainRoomData);
            ObjectiveRoomDataInst = Instantiate(ObjectiveRoomData);
            RoomDataInst = Instantiate(RoomData);

            foreach (RoomGraphicComponent graphicElement in RoomGraphicComponentDatas)
            {
                RoomGraphicComponentDatasInst.Add(Instantiate(graphicElement));
            }
        }
    }
}

