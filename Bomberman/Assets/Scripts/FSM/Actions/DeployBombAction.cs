﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="FSM/Actions/DeployBomb")]
public class DeployBombAction : Action {

    public override void Act(CreepFSM controller)
    {
        DeployBomb(controller);
    }

    private void DeployBomb(CreepFSM controller)
    {
        if(controller.creepData.currentBombs < controller.creepData.maxBombs)
        {
            GameObject instance = Instantiate(controller.bomb, Utilities.SnapToCell(controller.transform.position), Quaternion.identity);
            instance.GetComponent<Bomb>().creator = controller.gameObject;
        }
    }
}
