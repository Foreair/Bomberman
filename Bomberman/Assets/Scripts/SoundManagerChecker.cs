using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerChecker : MonoBehaviour {

    public GameObject SoundManagerPrefab;
	// Use this for initialization
	void Start () {
        if (SoundManager.instance == null)
            Instantiate(SoundManagerPrefab, Vector3.zero, Quaternion.identity);
	}
	
}
