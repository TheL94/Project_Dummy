using Framework.Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Generic
{
    [RequireComponent(typeof(LineRenderer))]
    public class PathRenderer : MonoBehaviour
    {
        public float VerticalOffSet;
        Vector3[] path;
        Vector3[] fullPath
        {
            get
            {
                List<Vector3> _fullPath = new List<Vector3>();
                _fullPath.Add(transform.position);
                _fullPath.AddRange(path);
                for (int i = 0; i < _fullPath.Count; i++)
                {
                    _fullPath[i] += new Vector3(0, VerticalOffSet, 0);
                }
                return _fullPath.ToArray();
            }
        }
        Dumby dumby;
        LineRenderer renderer;

        private void Start()
        {
            dumby = GetComponent<Dumby>();
            renderer = GetComponent<LineRenderer>();
            renderer.positionCount = 0;
            path = dumby.Path.GetRemainingSmoothedPath();
        }

        private void LateUpdate()
        {
            if(dumby.Path.GetOriginalPath().Length > 0)
            {
                path = dumby.Path.GetRemainingSmoothedPath();
                if (path!= null)
                {
                    renderer.positionCount = path.Length+1;
                    renderer.SetPositions(fullPath);
                }
            }
        }
    }
}
