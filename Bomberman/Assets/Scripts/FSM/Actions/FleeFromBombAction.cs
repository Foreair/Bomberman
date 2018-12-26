using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/FleeFromBomb")]
public class FleeFromBombAction : Action
{
    public LayerMask Mask;
    private GameObject bomb;
    private Vector2 direction = Vector2.zero;
    private bool bombInXAxis = false, bombInYAxis = false, bombOnTop = false;
    private int radiusExplosion;
    private float distanceToSafety = 0;


    public override void Act(CreepFSM controller)
    {

        radiusExplosion = controller.creepData.radiusExplosion;
        Flee(controller);
    }

    private void Flee(CreepFSM controller)
    {

        DetectDangerousBomb(controller);
        FleeFromBomb(controller);

    }


    private void DetectDangerousBomb(CreepFSM controller)
    {
        RaycastHit2D hit = Physics2D.BoxCast(Utilities.SnapToCell(controller.transform.position), new Vector2(0.9f, 0.9f), 0.0f, Vector2.zero, 0f);

        //RaycastHit2D hitUp = Physics2D.Raycast(controller.transform.position, Vector2.up);
        //RaycastHit2D hitRight = Physics2D.Raycast(controller.transform.position, Vector2.right);
        //RaycastHit2D hitDown = Physics2D.Raycast(controller.transform.position, Vector2.down);
        //RaycastHit2D hitLeft = Physics2D.Raycast(controller.transform.position, Vector2.left);
        //If there's a collider, it is a bomb and I am its creator
        if (hit.collider && hit.collider.CompareTag("Bomb") && hit.collider.gameObject.GetComponent<Bomb>().creator.CompareTag("Creep"))
        {
            bomb = hit.collider.gameObject;
            bombOnTop = true;
            return;
        }

        //if (hitUp.collider && hitUp.collider.CompareTag("Bomb") && hitUp.collider.gameObject.GetComponent<Bomb>().creator.CompareTag("Creep"))
        //{
        //    bombPosition = hitUp.collider.transform.position;
        //    bombInYAxis = true;
        //    return;
        //}

        //if (hitRight.collider && hitRight.collider.CompareTag("Bomb") && hitRight.collider.gameObject.GetComponent<Bomb>().creator.CompareTag("Creep"))
        //{
        //    bombPosition = hitRight.collider.transform.position;
        //    bombInXAxis = true;
        //    return;
        //}

        //if (hitDown.collider && hitDown.collider.CompareTag("Bomb") && hitDown.collider.gameObject.GetComponent<Bomb>().creator.CompareTag("Creep"))
        //{
        //    bombPosition = hitDown.collider.transform.position;
        //    bombInYAxis = true;
        //    return;
        //}

        //if (hitLeft.collider && hitLeft.collider.CompareTag("Bomb") && hitLeft.collider.gameObject.GetComponent<Bomb>().creator.CompareTag("Creep"))
        //{
        //    bombPosition = hitLeft.collider.transform.position;
        //    bombInXAxis = true;
        //    return;
        //}


    }
    private void FleeFromBomb(CreepFSM controller)
    {
        float distance = radiusExplosion + 1;
        if (bombOnTop)
        {
            if (direction == Vector2.zero)
            {
                direction = CheckDirections(controller);
                if (direction != Vector2.zero)
                    Move(controller);
            }
            else
            {
                distanceToSafety = Vector2.Distance(controller.transform.position, bomb.transform.position);
                if (distanceToSafety < distance)
                {
                    Move(controller);
                }
                else
                {
                    if (bomb)
                    {

                    }
                    direction = Vector2.zero;
                    distanceToSafety = 0;
                    bombOnTop = false;
                }
            }
        }
    }

    private Vector2 CheckDirections(CreepFSM controller)
    {
        Vector2 direction = Vector2.zero;
        float distance = radiusExplosion + 1;

        RaycastHit2D hitUp = Physics2D.Raycast(controller.transform.position, Vector2.up, distance, Mask);
        RaycastHit2D hitRight = Physics2D.Raycast(controller.transform.position, Vector2.right, distance, Mask);
        RaycastHit2D hitDown = Physics2D.Raycast(controller.transform.position, Vector2.down, distance, Mask);
        RaycastHit2D hitLeft = Physics2D.Raycast(controller.transform.position, Vector2.left, distance, Mask);

        //Up
        if (!hitUp.collider)
        {
            direction = Vector2.up;
            return direction;
        }
        //Right
        if (!hitRight.collider)
        {
            direction = Vector2.right;
            return direction;
        }
        //Down
        if (!hitDown.collider)
        {
            direction = Vector2.down;
            return direction;
        }
        //Left
        if (!hitLeft.collider)
        {
            direction = Vector2.left;
            return direction;
        }

        return direction;
    }

    private void Move(CreepFSM controller)
    {
        float currentMoved = Time.deltaTime * controller.creepData.speed;
        Vector2 localEndPos = new Vector2(direction.x * currentMoved, direction.y * currentMoved);
        Vector2 endPos = new Vector2(controller.transform.position.x + localEndPos.x, controller.transform.position.y + localEndPos.y);
        controller.rb.MovePosition(endPos);
    }
}
