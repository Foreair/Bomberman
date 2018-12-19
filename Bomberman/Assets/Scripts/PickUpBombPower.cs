using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBombPower : MonoBehaviour {

    int bombPowerIncrease = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.playerData.radiusExplosion += bombPowerIncrease;
            Destroy(gameObject);
        }
        else if (collision.tag == "Creep")
        {
            CreepFSM enemy = collision.gameObject.GetComponent<CreepFSM>();
            enemy.creepData.radiusExplosion += bombPowerIncrease;
            Destroy(gameObject);
        }
    }
}
