using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Use this for initialization
	void Start () {
        animator =  GetComponentInChildren<Animator>();
	}
	
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            animState = AnimationState.Running;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            animState = AnimationState.Idle;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            animState = AnimationState.Fallen;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            animState = AnimationState.Fighting;
	}

    public enum AnimationState
    {
        Fallen = 0,
        Idle = 1,
        Running = 2,
        Fighting = 3
    }
}
