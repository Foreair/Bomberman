using UnityEngine;
using System.Collections;

public class DestroyThings : MonoBehaviour
{
    //Delay in seconds before destroying the gameobject
    public float Delay = 1f;
    public bool destroyPickUps;
    private LayerMask Mask;

    void Start ()
    {
        Mask = LayerMask.GetMask("Power Ups");
        Destroy (gameObject, Delay);
        DestroyPickUps();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Die();
        }else if (collision.CompareTag("Creep"))
        {
            collision.gameObject.GetComponent<CreepFSM>().creepData.dead = true;
        }else if (collision.CompareTag("Bomb"))
        {
            collision.gameObject.GetComponent<Bomb>().CancelInvoke();
            collision.gameObject.GetComponent<Bomb>().Invoke("Explode", 0.0f);
        }
    }

    private void DestroyPickUps()
    {
        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, gameObject.GetComponent<BoxCollider2D>().size, 0.0f, Vector2.zero, 0.0f, Mask);
        foreach (var item in hit)
        {
            Destroy(item.collider.gameObject);
        }
    }
}
