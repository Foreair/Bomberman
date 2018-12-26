using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Decisions/FleeFromBombStop")]
public class FleeFromBombStopDecision : Decision {

    private LayerMask mask;
    public override bool Decide(CreepFSM controller)
    {
        mask = LayerMask.GetMask("Bombs");
        bool decision = CheckBombHasExploded(controller);
        return decision;
    }

    private bool CheckBombHasExploded(CreepFSM controller)
    {
        Collider2D result = Physics2D.OverlapBox(controller.transform.position, new Vector2(5,5), 0.0f, mask);
        if (!result)
            return true;
        return false;
    }
}
