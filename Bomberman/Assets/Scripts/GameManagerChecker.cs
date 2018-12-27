using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerChecker : MonoBehaviour {

    public GameObject GameManagerPrefab;
    // Use this for initialization
    void Start()
    {
        if (GameplayManager.instance == null)
            Instantiate(GameManagerPrefab, Vector3.zero, Quaternion.identity);
    }
}
