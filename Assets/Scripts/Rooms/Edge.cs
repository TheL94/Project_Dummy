using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Rooms.Cells
{
    public class Edge : MonoBehaviour
    {
        EdgeType _type = EdgeType.Wall;
        public EdgeType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        bool _isCollidingWithOtherEgdes;
        public bool IsCollidingWithOtherEgdes
        {
            get { return _isCollidingWithOtherEgdes; }
            set { _isCollidingWithOtherEgdes = value; }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Edge>() != null)
                IsCollidingWithOtherEgdes = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Edge>() != null)
                IsCollidingWithOtherEgdes = false;
        }
    }

    public enum EdgeType { Wall = 0, Door = 1 }
}