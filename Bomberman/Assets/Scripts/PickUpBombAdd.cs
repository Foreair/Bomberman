using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBombAdd : MonoBehaviour {

    int maxBombsIncreasement = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.playerData.maxBombs += maxBombsIncreasement;
            Destroy(gameObject);
        }
        else if (collision.tag == "Creep")
        {
            CreepFSM enemy = collision.gameObject.GetComponent<CreepFSM>();
            enemy.creepData.maxBombs += maxBombsIncreasement;
            Destroy(gameObject);
        }
    }
}
