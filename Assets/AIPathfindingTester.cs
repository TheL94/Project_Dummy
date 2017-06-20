﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;

public class AIPathfindingTester : MonoBehaviour {

    public GridController Grid;
    public Dumby dumby;

	void Start () {
        Grid.Setup();
	}
	
	void Update () {

		if(Input.GetKeyDown(KeyCode.Space))
        {
            dumby = Instantiate(dumby, Grid.GetGridCenter().WorldPosition, Quaternion.identity).GetComponent<Dumby>();
            dumby.Init();
        }
	}
}
