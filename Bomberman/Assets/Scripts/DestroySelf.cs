using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour
{
    //Delay in seconds before destroying the gameobject
    public float Delay = 1f;
    public bool destroyPickUps;

    void Start ()
    {
        Destroy (gameObject, Delay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Die();
        }else if (collision.CompareTag("Creep"))
        {
            collision.gameObject.GetComponent<CreepFSM>().creepData.dead = true;
        }
    }
}
