using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    //private Vector3 endPosition;
    private Vector2 endPosition2d;
    [SerializeField]
    private bool moving;
    private Rigidbody2D rb2d;
    private Transform mypos;
    private Vector2 movement = Vector2.zero;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        mypos = GetComponent<Transform>();
        moving = false;
    }


    private void Update()
    {
        UpdateMovement();
        
    }


    private void FixedUpdate()
    {
        if (moving)
        {
            rb2d.MovePosition(endPosition2d);
        }
    }

    private void UpdateMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0)
        {
            moving = true;

            if (movement.x > 0)
            {
                float realMoved = speed * Time.deltaTime;
                endPosition2d = new Vector2(mypos.position.x + realMoved, mypos.position.y);
                Debug.Log("Moving right");

            }
            else
            {
                float realMoved = speed * Time.deltaTime;
                endPosition2d = new Vector2(mypos.position.x - realMoved, mypos.position.y);
                Debug.Log("Moving left");
            }

        }
        if (movement.y != 0)
        {
            moving = true;
            if (movement.y > 0)
            {
                float realMoved = speed * Time.deltaTime;
                endPosition2d = new Vector2(mypos.position.x, mypos.position.y + realMoved);
                Debug.Log("Moving up");

            }
            else
            {
                float realMoved = speed * Time.deltaTime;
                endPosition2d = new Vector2(mypos.position.x, mypos.position.y - realMoved);
                Debug.Log("Moving up");
            }

        }

        if (!IsMoving(movement)) moving = false;
    }



    private bool IsMoving(Vector2 movement)
    {
        if (movement.x == 0 && movement.y == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
