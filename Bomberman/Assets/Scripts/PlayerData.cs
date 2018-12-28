using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData{

    //Public player variables
    [Header("Player Variables")]
    [Tooltip("Player's current number of lifes")]
    public int lifes;
    [Tooltip("Player's movement speed")]
    public float speed;
    [Tooltip("Maximum number of bombs the player can deploy")]
    public int maxBombs;
    [Tooltip("Radius of the explosion in each axis")]
    public int radiusExplosion;

    [HideInInspector] public int currentBombs;
    [HideInInspector] public bool dead;
    [HideInInspector] public bool moving;
    [HideInInspector] public Vector2 direction;

    public void InitializeData()
    {
        speed = 5.0f;
        maxBombs = 1;
        radiusExplosion = 1;
        currentBombs = 0;
        dead = false;
        moving = false;
        direction = Vector2.zero;
    }

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
