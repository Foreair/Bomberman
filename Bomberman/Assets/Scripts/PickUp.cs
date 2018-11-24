using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    public GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        GameObject instance = Instantiate(pickupEffect, transform.position, transform.rotation);


        Destroy(gameObject);
        //ParticleSystem p = instance.GetComponent<ParticleSystem>();

        //if (p.isStopped)
        //{
        //    Destroy(instance,p.main.duration);
        //}
    }
}
