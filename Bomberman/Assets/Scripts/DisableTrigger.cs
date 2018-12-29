using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTrigger : MonoBehaviour
{
    private BoxCollider2D mycollider;
    private GameObject parent;
    private LayerMask Mask;
    private Collider2D[] results = new Collider2D[5];
    private ContactFilter2D filter = new ContactFilter2D();
    private void Start()
    {
        mycollider = GetComponent<BoxCollider2D>();
        Mask = LayerMask.GetMask("Players") | LayerMask.GetMask("Enemies");
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(mycollider.OverlapCollider(filter, results) == 0)
            mycollider.isTrigger = false;
    }

}
