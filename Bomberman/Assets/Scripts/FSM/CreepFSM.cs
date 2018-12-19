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

        //float distance = 0.5f;
        //Vector3 size = new Vector3(0.9f, 0.9f, 0.9f);
        //Vector3 up = transform.position;
        //up.y += distance;
        //Vector3 down = transform.position;
        //down.y += distance;
        //Vector3 right = transform.position;
        //right.y += distance;
        //Vector3 left = transform.position;
        //left.y += distance;

        //Gizmos.DrawCube(up, size);
        //Gizmos.DrawCube(down, size);
        //Gizmos.DrawCube(right, size);
        //Gizmos.DrawCube(left, size);

    }


}
