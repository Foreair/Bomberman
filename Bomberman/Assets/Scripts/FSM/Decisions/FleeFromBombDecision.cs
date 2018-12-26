using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/FleeFromBomb")]
public class FleeFromBombDecision : Decision {
    GameObject bomb;
    public override bool Decide(CreepFSM controller)
    {
        bool decision = CheckBombInSight(controller);
        return decision;
    }

    private bool CheckBombInSight(CreepFSM controller)
    {
        RaycastHit2D hit = Physics2D.BoxCast(controller.transform.position, new Vector2(1,1), 0.0f, Vector2.zero, 0f);
        if (hit.collider && hit.collider.CompareTag("Bomb"))
        {
            //Check if it is mine
            if (hit.collider.gameObject.GetComponent<Bomb>().creator.CompareTag("Creep"))
            {
                return true;
            }

        }

        RaycastHit2D hitUp = Physics2D.Raycast(controller.transform.position, Vector2.up);
        RaycastHit2D hitRight = Physics2D.Raycast(controller.transform.position, Vector2.right);
        RaycastHit2D hitDown = Physics2D.Raycast(controller.transform.position, Vector2.down);
        RaycastHit2D hitLeft = Physics2D.Raycast(controller.transform.position, Vector2.left);

        if (hitUp.collider && hitUp.collider.CompareTag("Bomb"))
        {
            //Check if it is mine
            if (hitUp.collider.gameObject.GetComponent<Bomb>().creator.CompareTag("Creep"))
            {
                return true;
            }

        }

        if (hitRight.collider && hitRight.collider.CompareTag("Bomb"))
        {
            //Check if it is mine
            if (hitRight.collider.gameObject.GetComponent<Bomb>().creator.CompareTag("Creep"))
            {
                return true;
            }

        }

        if (hitDown.collider && hitDown.collider.CompareTag("Bomb"))
        {
            //Check if it is mine
            if (hitDown.collider.gameObject.GetComponent<Bomb>().creator.CompareTag("Creep"))
            {
                return true;
            }

        }

        if (hitLeft.collider && hitLeft.collider.CompareTag("Bomb"))
        {
            //Check if it is mine
            if (hitLeft.collider.gameObject.GetComponent<Bomb>().creator.CompareTag("Creep"))
            {
                return true;
            }

        }

        return false;
    }
}
