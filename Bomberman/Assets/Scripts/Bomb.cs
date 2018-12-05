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
        maxBombDistance = transform.parent.gameObject.GetComponent<PlayerController>().radiusExplosion;
        DWTilemap = GameObject.Find("Destroyable Walls").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Explode()
    {
        PlayerController player = gameObject.GetComponentInParent<PlayerController>();
        player.currentBombs--;

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
}
