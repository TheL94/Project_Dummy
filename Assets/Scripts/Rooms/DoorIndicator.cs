using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;

namespace DumbProject.Rooms
{
    // TODO : ottimizzare tutta la classe
    public class DoorIndicator : MonoBehaviour
    {
        GameObject pyramid;
        Material doorFrameMaterial;
        Door relativeDoor;

        public void Init(Door _relativeDoor)
        {
            relativeDoor = _relativeDoor;
            pyramid = GameManager.I.PoolMng.GetGameObject("Pyramid");
            pyramid.transform.position = relativeDoor.transform.position + new Vector3(0f, 3.5f, 0f);
            pyramid.transform.parent = relativeDoor.transform;

            MeshRenderer[] renderers = relativeDoor.GraphicElement.GetComponentsInChildren<MeshRenderer>();
            List<Material> materials = new List<Material>();
            if (renderers != null)
            {
                foreach (MeshRenderer mRend in renderers)
                {
                    if(materials != null)
                        materials.AddRange(mRend.materials.ToList());
                }
            }
            if (materials != null)
                doorFrameMaterial = materials.Find(m => m.name.Contains("IndicatorMaterial"));
        }

        private void Update()
        {
            if (relativeDoor == null || pyramid == null)
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

            MeshRenderer renderer = pyramid.GetComponentInChildren<MeshRenderer>();
            Material material = null;
            if (renderer == null)
                return;

            material = renderer.material;
            material.color = color;
            material.SetColor("_EmissionColor", color);

            if (doorFrameMaterial == null)
                return;
            doorFrameMaterial.color = color;
            doorFrameMaterial.SetColor("_EmissionColor", color);
        }

        public void DisableGraphic()
        {
            pyramid.transform.parent = null;
            pyramid.SetActive(false);
            GameManager.I.PoolMng.UpdatePools();
        }
    }
}

