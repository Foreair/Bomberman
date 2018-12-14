using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreepController : MonoBehaviour
{

    public struct MovementData
    {
        public bool IsMoving;
        public Vector2 direction;
        public float totalMoved;
        public float speed;
        public float currentMoved;

        public MovementData(bool Moving, Vector2 Dir, float AmountMoved, float Speed)
        {
            IsMoving = Moving;
            direction = Dir;
            totalMoved = AmountMoved;
            speed = Speed;
            currentMoved = 0.0f;
        }
    };

    public MovementData movement;
    private Rigidbody2D rb;
    private BoxCollider2D mycollider;
    private LayerMask Mask;
    private Vector2 endPos;

    // Use this for initialization
    void Start()
    {
        movement = new MovementData(false, Vector2.zero, 0.0f, 5.0f);
        rb = GetComponent<Rigidbody2D>();
        mycollider = GetComponent<BoxCollider2D>();
        Mask = LayerMask.GetMask("Walls") | LayerMask.GetMask("Destroyable Walls") | LayerMask.GetMask("Background");
    }

    public void Idle()
    {

    }

    public void Explore()
    {
        movement.currentMoved = Time.deltaTime * movement.speed;
        if (movement.totalMoved == 0.0f)
        {
            do
            {
                ChooseRandomDirection();
            } while (!CheckCollisions());

        }
        MoveCreep();

        //Reset variable
        movement.currentMoved = 0.0f;
    }

    private void ChooseRandomDirection()
    {
        //We choose a random direction to move
        int dir = Random.Range(1, 4);
        switch (dir)
        {
            case 1:
                //UP
                movement.direction = Vector2.up;
                endPos = new Vector2(transform.position.x, transform.position.y + movement.currentMoved);
                break;
            case 2:
                //RIGHT
                movement.direction = Vector2.right;
                endPos = new Vector2(transform.position.x + movement.currentMoved, transform.position.y);
                break;
            case 3:
                //DOWN
                movement.direction = Vector2.down;
                endPos = new Vector2(transform.position.x, transform.position.y - movement.currentMoved);
                break;
            case 4:
                //LEFT
                movement.direction = Vector2.left;
                endPos = new Vector2(transform.position.x - movement.currentMoved, transform.position.y);
                break;
        }
    }

    //True if we can move, false if there is a collision
    private bool CheckCollisions()
    {
        //Set IsMoving
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(mycollider.bounds.extents.x * 2, mycollider.bounds.extents.y * 2), 0.0f, movement.direction, 1.0f, Mask);

        if (hit.collider == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MoveCreep()
    {
        if (1 - movement.totalMoved < movement.currentMoved)
        {
            movement.currentMoved = 1 - movement.totalMoved;
            movement.totalMoved = 0.0f;
        }
        else
        {
            movement.totalMoved += movement.currentMoved;
        }

        rb.MovePosition(endPos);
    }
}
