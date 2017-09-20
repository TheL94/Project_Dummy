using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Rooms
{
    public class DoorIndicator : MonoBehaviour
    {
        GameObject objectToColor;
        Door relativeDoor;

        public void Init(Door _relativeDoor)
        {
            relativeDoor = _relativeDoor;
            objectToColor = Instantiate(objectToColor, relativeDoor.transform.position + new Vector3(0f, 3.5f, 0f), Quaternion.identity, relativeDoor.transform);
        }

        private void Update()
        {
            if (relativeDoor == null || objectToColor == null)
                return;

            ShowExplorationStatus();
        }

        public void ShowExplorationStatus()
        {
            Color color;
            switch (relativeDoor.StatusOfExploration)
            {
                case ExplorationStatus.NotInGame:
                    color = Color.white;
                    break;
                case ExplorationStatus.Unavailable:
                    color = Color.red;
                    break;
                case ExplorationStatus.NotExplored:
                    color = Color.yellow;
                    break;
                case ExplorationStatus.Explored:
                    color = Color.green;
                    break;
            }

            //objectToColor
        }
    }
}

