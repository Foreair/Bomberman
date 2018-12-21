using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTrigger : MonoBehaviour
{
    private BoxCollider2D mycollider;
    private GameObject parent;
    private void Start()
    {
        mycollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        mycollider.isTrigger = false;
    }

}
