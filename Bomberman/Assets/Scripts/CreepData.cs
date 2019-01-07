using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreepData {

    //Enemy info
    public float speed;
    public int maxBombs;
    public int radiusExplosion;
    public bool isImmortal = false;
    [HideInInspector] public int currentBombs;
    [HideInInspector] public bool dead;


    //Movement info
    [HideInInspector] public bool isMoving;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public bool horizontalMovement;

    public void InitializeData()
    {
        //Basic Creep Data
        speed = 5.0f;
        maxBombs = 1;
        radiusExplosion = 1;

        //Preset Data
        currentBombs = 0;
        dead = false;
        isMoving = false;
        horizontalMovement = false;
        direction = Vector2.zero;

        //Hacks
        isImmortal = false;
    }

}
