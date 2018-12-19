using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData{

    //Public player variables
    [Header("Player Variables")]
    [Tooltip("Player's current number of lifes")]
    public int lifes = 1;
    [Tooltip("Player's movement speed")]
    public float speed = 5.0f;
    [Tooltip("Maximum number of bombs the player can deploy")]
    public int maxBombs = 1;
    [Tooltip("Radius of the explosion in each axis")]
    public int radiusExplosion = 1;

    [HideInInspector] public int currentBombs = 0;
    [HideInInspector] public bool dead = false;
    [HideInInspector] public bool moving = false;
    [HideInInspector] public Vector2 direction = Vector2.zero;

    public bool Dead
    {
        get
        {
            return dead;
        }

        set
        {
            dead = value;
        }
    }

    public int CurrentBombs
    {
        get
        {
            return currentBombs;
        }

        set
        {
            currentBombs = value;
        }
    }
}
