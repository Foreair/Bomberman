using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreepData {

    //Enemy info
    public float speed = 5.0f;
    public int maxBombs = 1;
    public int radiusExplosion = 1;
    [HideInInspector] public int currentBombs = 0;

    //Movement info
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public Vector2 direction = Vector2.zero;
    [HideInInspector] public bool horizontalMovement = false;
    
}
