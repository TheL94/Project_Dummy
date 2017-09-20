using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    public class DoorIndicator : MonoBehaviour
    {
        GameObject objectToColor;
        Door relativeDoor;

        public void Init(Door _relativeDoor)
        {
            relativeDoor = _relativeDoor;
            objectToColor = GameManager.I.PoolMng.GetGameObject("Pyramid");
            objectToColor.transform.position = relativeDoor.transform.position + new Vector3(0f, 3.5f, 0f);
            objectToColor.transform.parent = relativeDoor.transform;
        }

        private void Update()
        {
            if (relativeDoor == null || objectToColor == null)
                return;

            ShowExplorationStatus();
        }

        public void ShowExplorationStatus()
        {
            Color color = new Color();
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

            MeshRenderer renderer = objectToColor.GetComponentInChildren<MeshRenderer>();
            Material material = null;
            if (renderer == null)
                return;

            material = renderer.material;
            material.color = color;
            material.SetColor("_EmissionColor", color);
        }

        private void OnDisable()
        {
            objectToColor.transform.parent = null;
            objectToColor.SetActive(false);
            GameManager.I.PoolMng.UpdatePools();
        }

        private void OnDestroy()
        {
            objectToColor.transform.parent = null;
            objectToColor.SetActive(false);
            GameManager.I.PoolMng.UpdatePools();
        }
    }
}

