using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{

    public float lifeTime = 3.0f;
    public GameObject explosionPrefab;

    [HideInInspector] public GameObject creator;
    private int maxBombDistance;
    private Tilemap DWTilemap;
    private LayerMask Mask;
    private Collider2D mycollider;

    // Use this for initialization
    void Start()
    {
        UpdateCurrentBombs(false);
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
        UpdateCurrentBombs(true);

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
        for (int i = 1; i == maxBombDistance; i++)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, direction, i, Mask);

            if (!hit2D.collider)
            {
                GameObject explosionInstance = Instantiate(explosionPrefab, new Vector2(transform.position.x, transform.position.y) + (i * direction), Quaternion.identity);
                explosionInstance.transform.parent = transform;
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
        if (creator.CompareTag("Player"))
        {
            maxBombDistance = creator.GetComponent<PlayerController>().playerData.radiusExplosion;
            return maxBombDistance;
        }else if (creator.CompareTag("Creep"))
        {
            maxBombDistance = creator.GetComponent<CreepFSM>().creepData.radiusExplosion;
        }
        else
        {
            //Error code
            maxBombDistance = -1;
            Debug.Log("The parent of the current bomb deployed is not supported or has its tag uncorrectly set");
        }

        return maxBombDistance;
    }

    private void UpdateCurrentBombs(bool hasExploded)
    {
        if (creator.CompareTag("Player"))
        {
            if (hasExploded)
            {
                creator.GetComponent<PlayerController>().playerData.CurrentBombs--;
            }
            else
            {
                creator.GetComponent<PlayerController>().playerData.CurrentBombs++;
            }
        }
        else if (creator.CompareTag("Creep"))
        {
            if (hasExploded)
            {
                creator.GetComponent<CreepFSM>().creepData.currentBombs--;
            }
            else
            {
                creator.GetComponent<CreepFSM>().creepData.currentBombs++;
            }
        }
        else
        {
            //Error code
            Debug.Log("The parent of the current bomb deployed is not supported or has its tag uncorrectly set.\n Update Current Bomb");
        }
    }
}
