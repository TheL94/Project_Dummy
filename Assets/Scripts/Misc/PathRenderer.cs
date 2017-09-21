using Framework.Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Generic
{
    [RequireComponent(typeof(LineRenderer))]
    public class PathRenderer : MonoBehaviour
    {
        Vector3[] path;
        Dumby dumby;
        LineRenderer renderer;

        private void Start()
        {
            dumby = GetComponent<Dumby>();
            renderer = GetComponent<LineRenderer>();
            renderer.positionCount = 0;
            path = dumby.Path.GetSmoothedPath();
        }

        private void LateUpdate()
        {
            path = dumby.Path.GetSmoothedPath();

            if(path!= null)
            {
                renderer.positionCount = path.Length;
                renderer.SetPositions(path);
            }
        }
    }
}
