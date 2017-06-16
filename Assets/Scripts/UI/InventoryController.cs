using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryController : MonoBehaviour {

    List<Slot> slots = new List<Slot>();

	// Use this for initialization
	void Start () {
        slots = GetComponentsInChildren<Slot>().ToList();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
