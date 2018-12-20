using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepFSM : MonoBehaviour
{

    public State currentState;
    public State remainState;
    public float stateTimeElapsed;
    public GameObject bomb;
    public CreepData creepData;
    [HideInInspector] public Grid grid;
    [HideInInspector] public Rigidbody2D rb;
    private Animator animator;

    public bool aiActive;

    private void Start()
    {
        grid = GameObject.FindObjectOfType<Grid>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        aiActive = false;
    }
    private void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
        UpdateAnimator();
    }

    public void UpdateAnimator()
    {
        animator.SetBool("moving", creepData.isMoving);
        animator.SetFloat("x", creepData.direction.x);
        animator.SetFloat("y", creepData.direction.y);
    }

    public void ChangeState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }

    private void OnDrawGizmos()
    {
        if (currentState != null)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = currentState.sceneGizmoColor;
        }
    }


}
