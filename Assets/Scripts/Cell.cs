using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cell : MonoBehaviour
{
    public List<Door> CellDoors = new List<Door>();
    bool IsInPosition;

    public void Setup()
    {
        CellDoors = GetComponentsInChildren<Door>().ToList();
    }
}
