using DumbProject.Grid;
using Framework.Pathfinding;
using UnityEngine;
using System.Collections.Generic;

public class Dumby : MonoBehaviour {

    Animator animator;
    AnimationState _animState = AnimationState.Idle;
    AnimationState animState
    {
        get { return _animState; }
        set
        {
            _animState = value;
            animator.SetInteger("AnimationState", (int)animState);
        }
    }

    GridController grid;
    GridNode currentNode { get { return grid.GetSpecificGridNode(transform.position); } }

    Pathfinder pathfinder;
    List<INetworkable> path;
    public float interactionRadius = 0.1f;

	// Use this for initialization
	public void Setup () {
        grid = FindObjectOfType<GridController>();
        pathfinder = new Pathfinder(currentNode);
        animator =  GetComponentInChildren<Animator>();
	}

    private void Update()
    {
            
    }
    
    public enum AnimationState
    {
        Fallen = 0,
        Idle = 1,
        Running = 2,
        Fighting = 3
    }
}
