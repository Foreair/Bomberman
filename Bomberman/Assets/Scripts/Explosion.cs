using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {


    public float lifeTime = 3.0f;

	// Use this for initialization
	void Start () {
        Invoke("Explode", lifeTime);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void Explode()
    {
        //PlayerController player = gameObject.GetComponentInParent(typeof(PlayerController)) as PlayerController;
        PlayerController player = gameObject.GetComponentInParent<PlayerController>();
        player.currentBombs--;
        Destroy(gameObject);
    }
}
