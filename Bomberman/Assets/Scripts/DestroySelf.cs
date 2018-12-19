using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour
{
    //Delay in seconds before destroying the gameobject
    public float Delay = 1f;

    void Start ()
    {
        Destroy (gameObject, Delay);
    }
}
