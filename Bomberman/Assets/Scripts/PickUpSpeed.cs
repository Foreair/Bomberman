using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpeed : MonoBehaviour
{

    float speedBoostAmount = 1.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.playerData.speed += speedBoostAmount;
            Destroy(gameObject);
        }else if (collision.tag == "Creep")
        {
            CreepFSM enemy = collision.gameObject.GetComponent<CreepFSM>();
            enemy.creepData.speed += speedBoostAmount;
            Destroy(gameObject);
        }
    }
}
