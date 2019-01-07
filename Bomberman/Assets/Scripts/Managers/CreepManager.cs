using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreepManager
{

    [HideInInspector] public Transform spawnPoint;          //Spawn location of this creep
    [HideInInspector] public GameObject instance;           //Instance of the Creep associated with this manager.
    [HideInInspector] public int wins;                      //Current number of wins for this player. In case we want to spawn enemies in the 2P mode

    private CreepFSM creepController;
    private CreepData creepData;

    public void Setup()
    {
        creepController = instance.GetComponent<CreepFSM>();
        creepData = creepController.creepData;

        creepData.InitializeData();
    }

    public void DisableControl()
    {
        creepController.enabled = false;
    }

    public void EnableControl()
    {
        creepController.enabled = true;
    }

    public void Reset()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;

        creepController.grid = GameplayManager.instance.map.GetComponent<Grid>();
        creepData.InitializeData();

        //Checkear si esto es útil y por qué
        instance.SetActive(false);
        instance.SetActive(true);
    }

}
