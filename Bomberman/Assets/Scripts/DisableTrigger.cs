using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTrigger : MonoBehaviour
{
    private BoxCollider2D mycollider;
    private GameObject creator;
    //private void Start()
    //{
    //    mycollider = GetComponent<BoxCollider2D>();
    //    creator = GetComponent<Bomb>().creator;
    //}

    //private void Update()
    //{
    //    if (mycollider.isTrigger)
    //        DeactivateTrigger();
    //}

    //private void DeactivateTrigger()
    //{
    //    if (!mycollider.IsTouching(creator.GetComponent<BoxCollider2D>()))
    //    {
    //        mycollider.isTrigger = false;
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    StartCoroutine(DeactivateTrigger());
    //}

    //IEnumerator DeactivateTrigger()
    //{
    //    yield return new WaitForSecondsRealtime(0.5f);
    //    GetComponent<Collider2D>().isTrigger = false;
    //}


}
