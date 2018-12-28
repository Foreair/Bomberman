using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public PlayerData playerData;
    public PlayerInput playerInput;

    [Space]
    [Header("Miscellaneous")]
    [Tooltip("Prefab containing the bomb")]
    public GameObject Bomb;

    //Private player variables
    private Vector2 endPosition2d;
    private float currentMoved;

    //Physics related variables
    private Rigidbody2D rb2d;
    private BoxCollider2D mycollider;
    private Vector2 boxSize;
    [Tooltip("Distance measuring how close the player can be against the environment")]
    public float offset;
    private LayerMask Mask, Mask2;

    //Animator
    private Animator animator;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        mycollider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        Mask = LayerMask.GetMask("Walls") | LayerMask.GetMask("Destroyable Walls") | LayerMask.GetMask("Background");
        Mask2 = LayerMask.GetMask("Bombs");
        boxSize = new Vector2(mycollider.bounds.extents.x * 2, mycollider.bounds.extents.y * 2);
        currentMoved = 0.0f;
        offset = 0.15f;
    }


    private void Update()
    {
        UpdateMovement();
        UpdateMechanics();

    }


    private void FixedUpdate()
    {
        UpdateAnimator();
        if (playerData.moving)
        {
            rb2d.MovePosition(endPosition2d);
            //Debug.Log("Velocity: x " + rb2d.velocity.x + ", y " + rb2d.velocity.y);
        }
    }

    private void UpdateMovement()
    {
        playerData.direction.x = Input.GetAxisRaw(playerInput.horizontalAxis);
        playerData.direction.y = Input.GetAxisRaw(playerInput.verticalAxis);

        //movement.x = Input.GetAxis("Horizontal");
        //movement.y = Input.GetAxis("Vertical");

        if (playerData.direction.magnitude == 0)
        {
            playerData.moving = false;
            currentMoved = 0.0f;
            return;
        }
        else
        {
            currentMoved = Time.deltaTime * playerData.speed;
        }

        if (!CheckCollisions())
        {
            playerData.moving = true;
            Vector2 localEndPos = new Vector2(playerData.direction.x * currentMoved, playerData.direction.y * currentMoved);
            endPosition2d = new Vector2(transform.position.x + localEndPos.x, transform.position.y + localEndPos.y);
        }
        else
        {
            playerData.moving = false;
            return;
        }
    }

    private void UpdateMechanics()
    {
        if (Input.GetButtonDown(playerInput.bombButton) && playerData.CurrentBombs < playerData.maxBombs)
        {

            GameObject instance = Instantiate(Bomb, Utilities.SnapToCell(transform.position), Bomb.transform.rotation);
            instance.GetComponent<Bomb>().creator = gameObject;
        }
    }

    public void Die()
    {
        playerData.Dead = true;
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    private void UpdateAnimator()
    {
        if (!playerData.moving)
        {
            animator.SetBool("moving", false);
        }
        else
        {
            animator.SetBool("moving", true);
        }

        animator.SetFloat("x", playerData.direction.x);
        animator.SetFloat("y", playerData.direction.y);
    }

    //True if we collide. False if we can move.
    private bool CheckCollisions()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0.0f, playerData.direction, offset, Mask);
        RaycastHit2D hitBomb = Physics2D.BoxCast(transform.position, boxSize, 0.0f, playerData.direction, offset, Mask2);

        //If we collide with a bomb, we do not move
        if(hitBomb.collider && !hitBomb.collider.isTrigger )
        {
            return true;
        }

        //If we dont collide we move
        if (!hit.collider)
        {
            return false;
        }

        //If we are moving in both directions and we  have collided, we check if we can move in either of those directions.
        //If we can, we set the direction accordingly, and return false
        if (playerData.direction.x != 0 && playerData.direction.y != 0)
        {
            Vector2 xOnly = new Vector2(playerData.direction.x, 0);
            Vector2 yOnly = new Vector2(0, playerData.direction.y);

            RaycastHit2D hitX = Physics2D.BoxCast(transform.position, boxSize, 0.0f, xOnly, offset, Mask);
            RaycastHit2D hitY = Physics2D.BoxCast(transform.position, boxSize, 0.0f, yOnly, offset, Mask);

            if (!hitX.collider)
            {
                playerData.direction = xOnly;
                return false;
            }

            if (!hitY.collider)
            {
                playerData.direction = yOnly;
                return false;
            }
        }
        return true;
    }

    //[UnityEditor.MenuItem("Tools/Increase Speed")]
    //public static void IncreaseSpeed()
    //{
    //    speed++;
    //}
}
