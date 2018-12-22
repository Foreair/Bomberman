using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/FleeFromBomb")]
public class FleeFromBombDecision : Decision {

    public override bool Decide(CreepFSM controller)
    {
        bool decision = CheckBombInSight(controller);
        return decision;
    }

    private bool CheckBombInSight(CreepFSM controller)
    {

        RaycastHit2D hitLeft = Physics2D.Raycast(controller.transform.position, Vector2.left);
        RaycastHit2D hitRight = Physics2D.Raycast(controller.transform.position, Vector2.right);
        RaycastHit2D hitUp = Physics2D.Raycast(controller.transform.position, Vector2.up);
        RaycastHit2D hitDown = Physics2D.Raycast(controller.transform.position, Vector2.down);

        if (hitLeft.collider)
        {
            if (hitLeft.collider.CompareTag("Bomb"))
            {
                return true;
            }
        }
        if (hitRight.collider)
        {
            if (hitRight.collider.CompareTag("Bomb"))
            {
                return true;
            }
        }
        if (hitUp.collider)
        {
            if (hitUp.collider.CompareTag("Bomb"))
            {
                return true;
            }
        }
        if (hitDown.collider)
        {
            if (hitDown.collider.CompareTag("Bomb"))
            {
                return true;
            }
        }

        return false;
    }
}
