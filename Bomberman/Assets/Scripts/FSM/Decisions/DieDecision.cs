using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/Die")]
public class DieDecision : Decision {

    public override bool Decide(CreepFSM controller)
    {
        bool die = controller.creepData.dead;
        return die;
    }
}
