using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CreepController))]
public class CreepAI : MonoBehaviour {

    //Player variables
    private GameObject player;
    private PlayerController playerController;

    //Creep variables
    private CreepController myController;
    public State currentState;
    private bool alive;

    //Variables for IDLE


    //Variables for EXPLORE


    public enum State
    {
        IDLE,
        EXPLORE
    }



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        myController = GetComponent<CreepController>();
        currentState = CreepAI.State.IDLE;
        alive = true;

        StartCoroutine("FSM");
    }

    IEnumerator FSM()
    {
        while (alive)
        {
            switch (currentState)
            {
                case State.IDLE:
                    Idle();
                    break;
                case State.EXPLORE:
                    Explore();
                    break;
            }
            yield return null;
        }
    }

    void Idle()
    {
        //myController.Idle();
        Debug.Log("Changing state to Explore from Idle");
        currentState = State.EXPLORE;
    }

    void Explore()
    {
        myController.Explore();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            playerController.Die();
        }
    }

}
