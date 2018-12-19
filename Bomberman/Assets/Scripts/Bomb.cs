using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{


    public float lifeTime = 3.0f;
    private int maxBombDistance;
    public GameObject explosionPrefab;
    private Tilemap DWTilemap;
    private LayerMask Mask;
    private Collider2D mycollider;

    // Use this for initialization
    void Start()
    {
        Invoke("Explode", lifeTime);
        Mask = LayerMask.GetMask("Walls") | LayerMask.GetMask("Destroyable Walls") | LayerMask.GetMask("Background");
        mycollider = GetComponent<Collider2D>();
        maxBombDistance = GetMaxBombDistance(gameObject);
        DWTilemap = GameObject.Find("Destroyable Walls").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Explode()
    {
        UpdateCurrentBombs(gameObject);

        GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosionInstance.transform.parent = transform;

        StartCoroutine(CreateExplosions(Vector2.up));
        StartCoroutine(CreateExplosions(Vector2.right));
        StartCoroutine(CreateExplosions(Vector2.down));
        StartCoroutine(CreateExplosions(Vector2.left));

        GetComponent<SpriteRenderer>().enabled = false;
        mycollider.enabled = false;
        Destroy(gameObject, .3f);
    }

    IEnumerator CreateExplosions(Vector2 direction)
    {
        for (int i = 0; i <= maxBombDistance; i++)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, direction, i, Mask);

            if (!hit2D.collider)
            {
                Instantiate(explosionPrefab, new Vector2(transform.position.x, transform.position.y) + (i * direction), Quaternion.identity);
            }
            else if (hit2D.collider.CompareTag("Destroyable Walls"))
            {
                Vector3 explosionPos = transform.position + new Vector3(direction.x * i, direction.y * i, 0);
                DWTilemap.SetTile(DWTilemap.WorldToCell(explosionPos), null);
                yield break;
            }

            yield return new WaitForSeconds(.05f);
        }
    }

    private int GetMaxBombDistance(GameObject gameObject)
    {
        int maxBombDistance;
        if (gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            maxBombDistance = gameObject.GetComponentInParent<PlayerController>().playerData.radiusExplosion;
            return maxBombDistance;
        }else if (gameObject.transform.parent.gameObject.CompareTag("Creep"))
        {
            maxBombDistance = gameObject.GetComponentInParent<CreepFSM>().creepData.radiusExplosion;
        }
        else
        {
            //Error code
            maxBombDistance = -1;
            Debug.Log("The parent of the current bomb deployed is not supported or has its tag uncorrectly set");
        }

        return maxBombDistance;
    }

    private void UpdateCurrentBombs(GameObject gameObject)
    {
        if (gameObject.transform.parent.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<PlayerController>().playerData.CurrentBombs--;
        }
        else if (gameObject.transform.parent.gameObject.CompareTag("Creep"))
        {
            gameObject.GetComponentInParent<CreepFSM>().creepData.currentBombs--;
        }
        else
        {
            //Error code
            Debug.Log("The parent of the current bomb deployed is not supported or has its tag uncorrectly set.\n Update Current Bomb");
        }
    }
}
