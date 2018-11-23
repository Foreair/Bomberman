using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    private Vector3 endPosition;
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

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        //movement.Normalize();

        if (movement.x != 0)
        {

            ////With forces
            //endPosition = new Vector2(mypos.position.x + movement.x, mypos.position.y);

            //Without forces
            endPosition = new Vector3(mypos.position.x + movement.x, mypos.position.y, mypos.position.z);
            mypos.SetPositionAndRotation(endPosition, mypos.rotation);

            moving = true;

            //Debug.Log("Moving left or right. End position: x: " + endPosition.x + " y: " + endPosition.y);
        }
        if (movement.y != 0)
        {
            //Without forces
            endPosition = new Vector3(mypos.position.x, mypos.position.y + movement.y, mypos.position.z);
            mypos.SetPositionAndRotation(endPosition, mypos.rotation);

        }
        else
        {
            moving = false;
        }
    }

    private bool IsMoving(Vector2 movement)
    {
        if (movement.x == 0 && movement.y == 0) {
            return false;
        }
        else
        {
            return true;
        }
    }

}
