using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreepController : MonoBehaviour
{
    [SerializeField]
    private bool isCentered;
    public struct MovementData
    {
        public bool IsMoving;
        public Vector2 direction;
        public float speed;
        public float currentMoved;
        public bool horizontalMovement;

        public MovementData(bool Moving, Vector2 Dir, float Speed)
        {
            IsMoving = Moving;
            direction = Dir;
            speed = Speed;
            currentMoved = 0.0f;
            horizontalMovement = false;
        }
    };

    public MovementData movement;
    private Rigidbody2D rb;
    private LayerMask Mask;
    private Vector2 endPos;

    private Grid grid;

    // Use this for initialization
    void Start()
    {
        movement = new MovementData(false, Vector2.zero, 5.0f);
        rb = GetComponent<Rigidbody2D>();
        Mask = LayerMask.GetMask("Walls") | LayerMask.GetMask("Destroyable Walls") | LayerMask.GetMask("Background");
        isCentered = false;
        grid = GameObject.FindObjectOfType<Grid>();
    }

    public void Idle()
    {

    }

    public void Explore()
    {
        isCentered = IsCentered(transform.position);
        movement.currentMoved = Time.deltaTime * movement.speed;
        //If we are centered and moving, we check if we want to change direction
        //If we are not centered but moving, we keep moving
        if (movement.IsMoving)
        {
            if (isCentered)
            {
                ChangeDirection();
                if (CheckCollisions())
                {
                    MoveCreep();
                }
            }
            else
            {
                if (CheckCollisions())
                {
                    MoveCreep();
                }
            }
        }
        //If we are not moving, we choose a random direction, we check for collisions and we move
        if (!movement.IsMoving)
        {
            ChooseRandomDirection();
            if (CheckCollisions())
            {
                MoveCreep();
            }
        }
        //Reset variable
        UpdateVariables();
    }

    private void ChangeDirection()
    {
        float distance = 1.0f;
        bool up = false, down = false, right = false, left = false;
        if (movement.horizontalMovement)
        {
            RaycastHit2D hitUp = Physics2D.BoxCast(transform.position, new Vector2(0.9f, 0.9f), 0.0f, Vector2.up, distance, Mask);
            RaycastHit2D hitDown = Physics2D.BoxCast(transform.position, new Vector2(0.9f, 0.9f), 0.0f, Vector2.down, distance, Mask);
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
                        movement.direction = Vector2.up;
                        movement.horizontalMovement = false;
                    }
                    else
                    {
                        movement.direction = Vector2.down;
                        movement.horizontalMovement = false;
                    }
                }
                else if (up)
                {
                    movement.direction = Vector2.up;
                    movement.horizontalMovement = false;
                }
                else if (down)
                {
                    movement.direction = Vector2.down;
                    movement.horizontalMovement = false;
                }
            }

        }
        else
        {
            RaycastHit2D hitRight = Physics2D.BoxCast(transform.position, new Vector2(0.9f, 0.9f), 0.0f, Vector2.right, distance, Mask);
            RaycastHit2D hitLeft = Physics2D.BoxCast(transform.position, new Vector2(0.9f, 0.9f), 0.0f, Vector2.left, distance, Mask);

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
                        movement.direction = Vector2.right;
                        movement.horizontalMovement = true;
                    }
                    else
                    {
                        movement.direction = Vector2.left;
                        movement.horizontalMovement = true;
                    }
                }
                else if (right)
                {
                    movement.direction = Vector2.right;
                    movement.horizontalMovement = true;
                }
                else if (left)
                {
                    movement.direction = Vector2.left;
                    movement.horizontalMovement = true;
                }
            }
        }
    }

    //Choses a random direction inbetween all 4 axis
    private void ChooseRandomDirection()
    {
        Debug.Log("Changing pos\n Current position x: " + transform.position.x + " y: " + transform.position.y);

        //We choose a random direction to move
        int dir = Random.Range(1, 5);
        switch (dir)
        {
            case 1:
                //UP
                movement.direction = Vector2.up;
                movement.horizontalMovement = false;
                break;
            case 2:
                //RIGHT
                movement.direction = Vector2.right;
                movement.horizontalMovement = true;
                break;
            case 3:
                //DOWN
                movement.direction = Vector2.down;
                movement.horizontalMovement = false;
                break;
            case 4:
                //LEFT
                movement.direction = Vector2.left;
                movement.horizontalMovement = true;
                break;
        }
    }

    //True if we can move, false if there is a collision
    //It also updates the IsMoving boolean
    private bool CheckCollisions()
    {
        //Set IsMoving
        float distance = 0.1f;
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(grid.cellSize.x * 0.9f, grid.cellSize.y * 0.9f), 0.0f, movement.direction, distance, Mask);

        if (hit.collider == null)
        {
            movement.IsMoving = true;
            return true;
        }
        else
        {
            movement.IsMoving = false;
            return false;
        }
    }

    //Checks whether the creep is centered inside a tile or not
    //True if centered. False if not.
    private bool IsCentered(Vector3 pos)
    {
        float threshold = 0.04f;
        int x = (int)pos.x;
        int y = (int)pos.y;

        float centeredX = Mathf.Abs(Mathf.Abs(pos.x - x) - grid.cellSize.x / 2);
        float centeredY = Mathf.Abs(Mathf.Abs(pos.y - y) - grid.cellSize.y / 2);

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
    private void MoveCreep()
    {

        Vector2 aux = new Vector2(movement.direction.x * movement.currentMoved, movement.direction.y * movement.currentMoved);
        endPos = new Vector2(transform.position.x + aux.x, transform.position.y + aux.y);
        rb.MovePosition(endPos);

    }

    private void UpdateVariables()
    {
        movement.currentMoved = 0.0f;
    }
}
