using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "FSM/Actions/Explore")]
public class ExploreAction : Action {

    private bool isCentered;
    private float currentMoved;

    private LayerMask Mask;
    private Vector2 endPos;

    private void Awake()
    {
        Mask = LayerMask.GetMask("Walls") | LayerMask.GetMask("Destroyable Walls") | LayerMask.GetMask("Background") | LayerMask.GetMask("Bombs");
        isCentered = false;
    }
    public override void Act(CreepFSM controller)
    {
        Explore(controller);
    }

    public void Explore(CreepFSM controller)
    {
        isCentered = IsCentered(controller, controller.transform.position);
        currentMoved = Time.deltaTime * controller.creepData.speed;
        //If we are centered and moving, we check if we want to change direction
        //If we are not centered but moving, we keep moving
        if (controller.creepData.isMoving)
        {
            if (isCentered)
            {
                ChangeDirection(controller);
                if (CheckCollisions(controller))
                {
                    MoveCreep(controller);
                }
            }
            else
            {
                if (CheckCollisions(controller))
                {
                    MoveCreep(controller);
                }
            }
        }
        //If we are not moving, we choose a random direction, we check for collisions and we move
        if (!controller.creepData.isMoving)
        {
            ChooseRandomDirection(controller);
            if (CheckCollisions(controller))
            {
                MoveCreep(controller);
            }
        }
        //Reset variable
        UpdateVariables(controller);
    }

    private void ChangeDirection(CreepFSM controller)
    {
        float distance = 1.0f;
        bool up = false, down = false, right = false, left = false;
        if (controller.creepData.horizontalMovement)
        {
            RaycastHit2D hitUp = Physics2D.BoxCast(controller.transform.position, new Vector2(0.9f, 0.9f), 0.0f, Vector2.up, distance, Mask);
            RaycastHit2D hitDown = Physics2D.BoxCast(controller.transform.position, new Vector2(0.9f, 0.9f), 0.0f, Vector2.down, distance, Mask);
            if (hitUp.collider == null)
                up = true;
            if (hitDown.collider == null)
                down = true;
            //If we cant go up nor down, we stop trying to change dir.
            if (!up && !down)
                return;

            //50% chance to change direction. If we get 1, we change direction
            int changeDir = Random.Range(0, 2);
            if (changeDir == 1)
            {
                //Checking whether both options are available or only one
                if (up && down)
                {
                    int dir = Random.Range(0, 2);
                    if (dir == 0)
                    {
                        controller.creepData.direction = Vector2.up;
                        controller.creepData.horizontalMovement = false;
                    }
                    else
                    {
                        controller.creepData.direction = Vector2.down;
                        controller.creepData.horizontalMovement = false;
                    }
                }
                else if (up)
                {
                    controller.creepData.direction = Vector2.up;
                    controller.creepData.horizontalMovement = false;
                }
                else if (down)
                {
                    controller.creepData.direction = Vector2.down;
                    controller.creepData.horizontalMovement = false;
                }
            }

        }
        else
        {
            RaycastHit2D hitRight = Physics2D.BoxCast(controller.transform.position, new Vector2(0.9f, 0.9f), 0.0f, Vector2.right, distance, Mask);
            RaycastHit2D hitLeft = Physics2D.BoxCast(controller.transform.position, new Vector2(0.9f, 0.9f), 0.0f, Vector2.left, distance, Mask);

            if (hitRight.collider == null)
                right = true;
            if (hitLeft.collider == null)
                left = true;
            //If we cant go up nor down, we stop trying to change dir.
            if (!right && !left)
                return;

            //50% chance to change direction. If we get 1, we change direction
            int changeDir = Random.Range(0, 2);
            if (changeDir == 1)
            {
                //Checking whether both options are available or only one
                if (right && left)
                {
                    int dir = Random.Range(0, 2);
                    if (dir == 0)
                    {
                        controller.creepData.direction = Vector2.right;
                        controller.creepData.horizontalMovement = true;
                    }
                    else
                    {
                        controller.creepData.direction = Vector2.left;
                        controller.creepData.horizontalMovement = true;
                    }
                }
                else if (right)
                {
                    controller.creepData.direction = Vector2.right;
                    controller.creepData.horizontalMovement = true;
                }
                else if (left)
                {
                    controller.creepData.direction = Vector2.left;
                    controller.creepData.horizontalMovement = true;
                }
            }
        }
    }

    //Choses a random direction inbetween all 4 axis
    private void ChooseRandomDirection(CreepFSM controller)
    {
        Debug.Log("Changing pos\n Current position x: " + controller.transform.position.x + " y: " + controller.transform.position.y);

        //We choose a random direction to move
        int dir = Random.Range(1, 5);
        switch (dir)
        {
            case 1:
                //UP
                controller.creepData.direction = Vector2.up;
                controller.creepData.horizontalMovement = false;
                break;
            case 2:
                //RIGHT
                controller.creepData.direction = Vector2.right;
                controller.creepData.horizontalMovement = true;
                break;
            case 3:
                //DOWN
                controller.creepData.direction = Vector2.down;
                controller.creepData.horizontalMovement = false;
                break;
            case 4:
                //LEFT
                controller.creepData.direction = Vector2.left;
                controller.creepData.horizontalMovement = true;
                break;
        }
    }

    //True if we can move, false if there is a collision
    //It also updates the IsMoving boolean
    private bool CheckCollisions(CreepFSM controller)
    {
        //Set IsMoving
        float distance = 0.1f;
        RaycastHit2D hit = Physics2D.BoxCast(controller.transform.position, new Vector2(controller.grid.cellSize.x * 0.9f, controller.grid.cellSize.y * 0.9f), 0.0f, controller.creepData.direction, distance, Mask);

        if (hit.collider == null || hit.collider.isTrigger)
        {
            controller.creepData.isMoving = true;
            return true;
        }
        else
        {
            controller.creepData.isMoving = false;
            return false;
        }
    }

    //Checks whether the creep is centered inside a tile or not
    //True if centered. False if not.
    private bool IsCentered(CreepFSM controller, Vector3 pos)
    {
        float threshold = 0.04f;
        int x = (int)pos.x;
        int y = (int)pos.y;

        float centeredX = Mathf.Abs(Mathf.Abs(pos.x - x) - controller.grid.cellSize.x / 2);
        float centeredY = Mathf.Abs(Mathf.Abs(pos.y - y) - controller.grid.cellSize.y / 2);

        if (centeredX < threshold && centeredY < threshold)
        {
            //Debug.Log("Res: " + res);
            return true;
        }
        else
        {
            return false;
        }

    }
    //Moves the rigidbody component
    private void MoveCreep(CreepFSM controller)
    {

        Vector2 localEndPos = new Vector2(controller.creepData.direction.x * currentMoved, controller.creepData.direction.y * currentMoved);
        endPos = new Vector2(controller.transform.position.x + localEndPos.x, controller.transform.position.y + localEndPos.y);
        controller.rb.MovePosition(endPos);

    }

    private void UpdateVariables(CreepFSM controller)
    {
        currentMoved = 0.0f;
    }
}
