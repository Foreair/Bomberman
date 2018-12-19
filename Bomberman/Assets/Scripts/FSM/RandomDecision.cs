using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/RandomDecision")]
public class RandomDecision : Decision {

    public override bool Decide(CreepFSM controller)
    {
        int decision = Random.Range(0, 2);
        bool result = decision == 1 ? true : false;
        return result;
    }
}
