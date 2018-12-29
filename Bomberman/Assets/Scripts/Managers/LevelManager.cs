using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {

    [System.Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;

    public Count wallCount = new Count(2, 5);
    public Count destroyableWallCount = new Count(2, 5);
    public Count pickUpsCount = new Count(2, 5);
    public Tile[] floorTiles; 
    public Tile[] wallTiles; 
    public Tile[] destroyableWallTiles; 
    public Tile[] backgroundTiles; 
    public GameObject[] pickUps;

    private GameObject mapHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for(int x = 0; x < columns; x++)
        {
            for(int y = 0; y < rows; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void GenerateBasicMap()
    {
        //Creating the map holder parent which contains the grid.
        mapHolder = new GameObject("Map");
        mapHolder.AddComponent<Grid>();

        //Creating background and floor Gameobjects and setting their layers/tags
        GameObject background = new GameObject("Background");
        background.tag = "Background";
        background.layer = LayerMask.NameToLayer("Background");

        GameObject floor = new GameObject("Ground");
        floor.tag = "Ground";
        floor.layer = LayerMask.NameToLayer("Ground");

        //Setting mapHolder as their parent
        background.transform.SetParent(mapHolder.transform);
        floor.transform.SetParent(mapHolder.transform);

        //Adding the tilemap and tilemapRenderer components and setting their sorting layer.
        Tilemap backgroundTilemap = background.AddComponent<Tilemap>();
        TilemapRenderer backgroundTilemapRenderer = background.AddComponent<TilemapRenderer>();
        backgroundTilemapRenderer.sortingLayerName = "Background";
        Tilemap floorTilemap = floor.AddComponent<Tilemap>();
        TilemapRenderer floorTilemapRenderer = floor.AddComponent<TilemapRenderer>();
        floorTilemapRenderer.sortingLayerName = "Ground";

        //Initialising Tiles
        InitialiseTiles(true, backgroundTilemap, floorTilemap);

        //Adding colliders
        Rigidbody2D rb2D = background.AddComponent<Rigidbody2D>();
        rb2D.bodyType = RigidbodyType2D.Static;
        TilemapCollider2D tilemapCollider2D = background.AddComponent<TilemapCollider2D>();
        tilemapCollider2D.usedByComposite = true;
        background.AddComponent<CompositeCollider2D>();
    }

    private void InitialiseTiles(bool centered, Tilemap bgTilemap, Tilemap floorTilemap)
    {
        ////Attempt to do it more efficient
        //BoundsInt box = new BoundsInt(Vector3Int.zero, new Vector3Int(columns, rows, 0));
        //TileBase[] ground = new Tile[columns * rows];
        //for (int i = 0; i < ground.Length; i++)
        //{
        //    ground[i] = floorTiles[0];
        //}

        //backgroundTilemap.SetTilesBlock(box, ground);

        if (centered)
        {
            for (int x = (-columns / 2) - 1; x < (columns / 2) + 1; x++)
            {
                for (int y = (-rows / 2) - 1; y < (rows / 2) + 1; y++)
                {
                    Tile toInstantiate;
                    //If we are in the background
                    if (x == (-columns / 2) - 1 || x == (columns / 2) || y == (-rows / 2) - 1 || y == (rows / 2))
                    {

                        toInstantiate = backgroundTiles[Random.Range(0, backgroundTiles.Length)];
                        bgTilemap.SetTile(bgTilemap.WorldToCell(new Vector3Int(x, y, 0)), toInstantiate);
                    }
                    else
                    {
                        toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                        floorTilemap.SetTile(floorTilemap.WorldToCell(new Vector3Int(x, y, 0)), toInstantiate);
                    }
                }
            }
        }
        else
        {
            for (int x = - 1; x < columns + 1; x++)
            {
                for (int y = - 1; y < rows + 1; y++)
                {
                    Tile toInstantiate;
                    //If we are in the background
                    if (x == - 1 || x == columns || y == - 1 || y == rows)
                    {

                        toInstantiate = backgroundTiles[Random.Range(0, backgroundTiles.Length)];
                        bgTilemap.SetTile(bgTilemap.WorldToCell(new Vector3Int(x, y, 0)), toInstantiate);
                    }
                    else
                    {
                        toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                        floorTilemap.SetTile(floorTilemap.WorldToCell(new Vector3Int(x, y, 0)), toInstantiate);
                    }
                }
            }
        }
    }

    private Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    private void LayoutObjectAtRandom(Tile[] tileArray, int minimum, int maximum)
    {
        for (int j = 0; j < rows; j++)
        {
            int objectCount = Random.Range(minimum, maximum + 1);
            for (int i = 0; i < objectCount; i++)
            {
                Vector3 randomPosition = RandomPosition();
                GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
                Instantiate(tileChoice, randomPosition, Quaternion.identity);
            }
        }
    }

    public void SetupScene(int level)
    {
        GenerateBasicMap();
        //InitialiseList();
        //LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        //LayoutObjectAtRandom(destroyableWallTiles, destroyableWallCount.minimum, destroyableWallCount.maximum);
        //LayoutObjectAtRandom(pickUps, pickUpsCount.minimum, pickUpsCount.maximum);

    }

}
