using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    //Public player variables
    [Header("Player Variables")]
    [Tooltip("Player's movement speed")]
    public float speed = 5;
    [Tooltip("Maximum number of bombs the player can deploy")]
    public int maxBombs = 1;
    [Tooltip("Radius of the explosion in each axis")]
    public int radiusExplosion = 1;

    [Space] [Header("Miscellaneous")]
    [Tooltip("Prefab containing the bomb")]
    public GameObject Bomb;
    [Tooltip("Grid containing all the tilemaps from the current level")]
    public Grid grid;
    //[HideInInspector]

    //Private player variables
    private int currentBombs = 0;
    private bool dead = false;
    private bool moving;
    private Vector2 movement = Vector2.zero;
    private Vector2 endPosition2d;

    //Physics related variables
    private Rigidbody2D rb2d;
    private BoxCollider2D mycollider;
    private Vector2 rightUp, rightDown, leftUp, leftDown;
    private float offset = 0.05f;
    private LayerMask Mask;

    //Animator
    private Animator animator;



    public bool Dead
    {
        get
        {
            return dead;
        }

        set
        {
            dead = value;
        }
    }

    public int CurrentBombs
    {
        get
        {
            return currentBombs;
        }

        set
        {
            currentBombs = value;
        }
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        mycollider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        Mask = LayerMask.GetMask("Walls") | LayerMask.GetMask("Destroyable Walls") | LayerMask.GetMask("Background");
        moving = false;
    }


    private void Update()
    {
        UpdateMovement();
        UpdateMechanics();

    }


    private void FixedUpdate()
    {
        UpdateAnimator();
        if (moving)
        {
            rb2d.MovePosition(endPosition2d);
            //Debug.Log("Velocity: x " + rb2d.velocity.x + ", y " + rb2d.velocity.y);
        }
    }

    private void UpdateMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //movement.x = Input.GetAxis("Horizontal");
        //movement.y = Input.GetAxis("Vertical");

        moving = false;

        rightUp = new Vector2(transform.position.x + mycollider.bounds.extents.x, transform.position.y + mycollider.bounds.extents.y);
        rightDown = new Vector2(transform.position.x + mycollider.bounds.extents.x, transform.position.y - mycollider.bounds.extents.y);
        leftUp = new Vector2(transform.position.x - mycollider.bounds.extents.x, transform.position.y + mycollider.bounds.extents.y);
        leftDown = new Vector2(transform.position.x - mycollider.bounds.extents.x, transform.position.y - mycollider.bounds.extents.y);


        if (movement.x != 0)
        {
            if (movement.x > 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(rightUp, new Vector2(movement.x, 0), offset, Mask);
                RaycastHit2D hit2 = Physics2D.Raycast(rightDown, new Vector2(movement.x, 0), offset, Mask);

                if (hit.collider || hit2.collider)
                    moving = false;
                else
                {
                    float realMoved = speed * Time.deltaTime;
                    endPosition2d = new Vector2(transform.position.x + realMoved, transform.position.y);
                    Debug.Log("Moving right");
                    moving = true;
                }

            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(leftUp, new Vector2(movement.x, 0), offset, Mask);
                RaycastHit2D hit2 = Physics2D.Raycast(leftDown, new Vector2(movement.x, 0), offset, Mask);

                if (hit.collider || hit2.collider)
                    moving = false;
                else
                {
                    float realMoved = speed * Time.deltaTime;
                    endPosition2d = new Vector2(transform.position.x - realMoved, transform.position.y);
                    Debug.Log("Moving left");
                    moving = true;
                }
            }

        }
        if (movement.y != 0)
        {
            if (movement.y > 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(rightUp, new Vector2(0, movement.y), offset, Mask);
                RaycastHit2D hit2 = Physics2D.Raycast(leftUp, new Vector2(0, movement.y), offset, Mask);

                if (hit.collider || hit2.collider)
                    moving = false;
                else
                {
                    float realMoved = speed * Time.deltaTime;
                    endPosition2d = new Vector2(transform.position.x, transform.position.y + realMoved);
                    Debug.Log("Moving up");
                    moving = true;
                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(rightDown, new Vector2(0, movement.y), offset, Mask);
                RaycastHit2D hit2 = Physics2D.Raycast(leftDown, new Vector2(0, movement.y), offset, Mask);

                if (hit.collider || hit2.collider)
                    moving = false;
                else
                {
                    float realMoved = speed * Time.deltaTime;
                    endPosition2d = new Vector2(transform.position.x, transform.position.y - realMoved);
                    Debug.Log("Moving down");
                    moving = true;
                }
            }
        }
    }

    private void UpdateMechanics()
    {
        if (Input.GetButtonDown("Jump") && CurrentBombs < maxBombs)
        {

            GameObject instance = Instantiate(Bomb, SnapBomb(transform.position), Bomb.transform.rotation);
            instance.transform.parent = transform;
            CurrentBombs++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Explosion"))
        {
            Die();
        }

        if(collision.CompareTag("Bomb Power")){
            Destroy(collision.gameObject);
            maxBombs++;
        }

        if (collision.CompareTag("Boost Speed")){
            Destroy(collision.gameObject);
            speed++;
        }
        if (collision.CompareTag("Boost Explosion"))
        {
            Destroy(collision.gameObject);
            radiusExplosion++;
        }
    }

    public void Die()
    {
        Dead = true;
        Destroy(gameObject);
    }
    static public Vector3 SnapBomb(Vector3 pos)
    {
        Grid grid = FindObjectOfType<Grid>();
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


    private void UpdateAnimator()
    {
        if (!moving)
        {
            animator.SetBool("moving", false);
        }
        else
        {
            animator.SetBool("moving", true);
        }

        animator.SetFloat("x", movement.x);
        animator.SetFloat("y", movement.y);
    }

    //[UnityEditor.MenuItem("Tools/Increase Speed")]
    //public static void IncreaseSpeed()
    //{
    //    speed++;
    //}
}
