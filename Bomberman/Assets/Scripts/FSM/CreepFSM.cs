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

    private bool aiActive;

    private void Start()
    {
        grid = GameObject.FindObjectOfType<Grid>();
        rb = GetComponent<Rigidbody2D>();
        aiActive = true;
    }
    private void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
    }

    public void ChangeState(State nextState)
    {
        if (nextState != currentState)
        {
            currentState = nextState;
        }
    }

    private void OnDrawGizmos()
    {
        if (currentState != null)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = currentState.sceneGizmoColor;
        }
    }


}
