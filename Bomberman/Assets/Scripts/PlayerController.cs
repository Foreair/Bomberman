using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [Header("Player Variables")]
    [Tooltip("Player's movement speed")]
    public float speed = 5;
    [Tooltip("Maximum number of bombs the player can deploy")]
    public int maxBombs = 1;
    //[HideInInspector]
    public int currentBombs = 0;
    public bool dead = false;
    private Vector2 endPosition2d;
    private bool moving;
    private Rigidbody2D rb2d;
    private Vector2 movement = Vector2.zero;
    [Space]
    public GameObject Bomb;
    public Grid grid;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        moving = false;
    }


    private void Update()
    {
        UpdateMovement();
        UpdateMechanics();

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
                endPosition2d = new Vector2(transform.position.x + realMoved, transform.position.y);
                Debug.Log("Moving right");

            }
            else
            {
                float realMoved = speed * Time.deltaTime;
                endPosition2d = new Vector2(transform.position.x - realMoved, transform.position.y);
                Debug.Log("Moving left");
            }

        }
        if (movement.y != 0)
        {
            moving = true;
            if (movement.y > 0)
            {
                float realMoved = speed * Time.deltaTime;
                endPosition2d = new Vector2(transform.position.x, transform.position.y + realMoved);
                Debug.Log("Moving up");

            }
            else
            {
                float realMoved = speed * Time.deltaTime;
                endPosition2d = new Vector2(transform.position.x, transform.position.y - realMoved);
                Debug.Log("Moving up");
            }

        }

        if (!IsMoving(movement)) moving = false;
    }

    private void UpdateMechanics()
    {
        if (Input.GetButtonDown("Jump") && currentBombs < maxBombs)
        {

            GameObject instance = Instantiate(Bomb, SnapBomb(transform.position), Bomb.transform.rotation);
            instance.transform.parent = transform;
            currentBombs++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Explosion"))
        {
            dead = true;
            Destroy(gameObject);
        }

        if(collision.CompareTag("Bomb Power")){
            Destroy(collision.gameObject);
            maxBombs++;
        }

        if (collision.CompareTag("Boost Speed")){
            Destroy(collision.gameObject);
            speed++;
        }
    }



    public bool IsMoving(Vector2 movement)
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

    private Vector3 SnapBomb(Vector3 pos)
    {
        Vector3 snappedPos = Vector3.zero;
        if(pos.x > 0)
        {
            snappedPos.x = (int) pos.x + (grid.cellSize.x/2);
        }
        else
        {
            snappedPos.x = (int)pos.x - (grid.cellSize.x / 2);
        }

        if (pos.y > 0)
        {
            snappedPos.y = (int)pos.y + (grid.cellSize.y / 2);
        }
        else
        {
            snappedPos.y = (int)pos.y - (grid.cellSize.y / 2);
        }

        return snappedPos;
    }

    //[UnityEditor.MenuItem("Tools/Increase Speed")]
    //public static void IncreaseSpeed()
    //{
    //    speed++;
    //}
}
