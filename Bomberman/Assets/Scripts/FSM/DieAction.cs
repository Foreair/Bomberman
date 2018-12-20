using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/Die")]
public class DieAction : Action {
    public float destroyDelay = 0.50f;
    public override void Act(CreepFSM controller)
    {
        controller.creepData.isMoving = false;
        controller.creepData.direction = Vector2.zero;
        Destroy(controller, destroyDelay);
    }
}
