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
    GridNode currentNode;

    Pathfinder pathfinder;
    List<INetworkable> path;
    public float interactionRadius = 0.1f;

	// Use this for initialization
	public void Init () {
        grid = FindObjectOfType<GridController>();
        currentNode = grid.GetSpecificGridNode(transform.position);
        pathfinder = new Pathfinder(currentNode);
        animator =  GetComponentInChildren<Animator>();
	}	
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Keypad1))
           path = pathfinder.FindPath(grid.GetSpecificGridNode(new GridPosition(0, 0)), currentNode);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            animState = AnimationState.Running;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            animState = AnimationState.Idle;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            animState = AnimationState.Fallen;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            animState = AnimationState.Fighting;

        if (path != null)
            WalkTroughtNodes(path);
	}
    
    void WalkTroughtNodes(List<INetworkable> _path)
    {
        INetworkable actualObjective = _path[0];

        transform.LookAt(actualObjective.spacePosition);
        transform.position = actualObjective.spacePosition;

        if (Vector3.Distance(currentNode.WorldPosition, actualObjective.spacePosition) <= interactionRadius)
        {
            if (_path.Count == 1)
                _path = new List<INetworkable>();
            else
                _path.Remove(_path[0]);
        }
    }

    public enum AnimationState
    {
        Fallen = 0,
        Idle = 1,
        Running = 2,
        Fighting = 3
    }
}
