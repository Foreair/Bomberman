using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{

    [System.Serializable]
    //Simple class to specify a range and hold it on an instance of the class
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
    //Main holders of our map
    private GameObject map;                                     //Reference for our map Gameobject
    private GameObject background;                              //Reference for our background Gameobject
    private GameObject floor;                                   //Reference for our floor Gameobject
    private GameObject walls;                                   //Reference for our walls Gameobject
    private GameObject destroyableWalls;                        //Reference for our destroyableWalls Gameobject
    private GameObject powerups;                                //Reference for our powerups Gameobject
    private GameObject boostSpeed;                              //Reference for our powerups Gameobject
    private GameObject bombIncreasement;                        //Reference for our powerups Gameobject
    private GameObject boostExplosion;                          //Reference for our powerups Gameobject

    //Easy accesible needed tilemaps
    private Tilemap mapTilemap;                                 //Reference for our map Tilemap
    private Tilemap backgroundTilemap;                          //Reference for our background Tilemap
    private Tilemap floorTilemap;                               //Reference for our floor Tilemap
    private Tilemap wallsTilemap;                               //Reference for our walls Tilemap
    private Tilemap destroyableWallsTilemap;                    //Reference for our destroyableWalls Tilemap


    public bool isCentered = false;                             //Specifies if we generate the map starting from the 0,0 axis or we create it around the 0,0.
    public int columns = 8;                                     //columns of our map
    public int rows = 8;                                        //rows of our map

    private Count wallCount;                                    //Holds the range of how many walls do we want in each column
    private Count destroyableWallCount;                         //Holds the range of how many destroyableWalls do we want in each column
    public Count pickUpsCount = new Count(2, 5);                //Holds the range of how many pickups do we want in each column
    public Tile[] floorTiles;                                   //Reference for different floorTiles to use
    public Tile[] wallTiles;                                    //Reference for different wallTiles to use
    public Tile[] destroyableWallTiles;                         //Reference for different destroyableWallTiles to use
    public Tile[] backgroundTiles;                              //Reference for different backgroundTiles to use
    public GameObject[] pickUps;                                //Reference for different pickUps to use

    private int startPosCol, endPosCol, startPosRow, endPosRow;

    //Used to generate the base map 
    //Sets the boundaries of our level and the walls restraining the movement to inside the level
    private void GenerateBasicMap()
    {
        ////Attempt to do it more efficient
        //BoundsInt box = new BoundsInt(Vector3Int.zero, new Vector3Int(columns, rows, 0));
        //TileBase[] ground = new Tile[columns * rows];
        //for (int i = 0; i < ground.Length; i++)
        //{
        //    ground[i] = floorTiles[0];
        //}

        //backgroundTilemap.SetTilesBlock(box, ground);
        if (isCentered)
        {
            for (int x = startPosCol - 1; x < endPosCol + 1; x++)
            {
                for (int y = startPosRow - 1; y < endPosRow + 1; y++)
                {
                    Tile toInstantiate;
                    //If we are in the background
                    if (x == startPosCol - 1 || x == endPosCol || y == startPosRow - 1 || y == endPosRow)
                    {

                        toInstantiate = backgroundTiles[Random.Range(0, backgroundTiles.Length)];
                        backgroundTilemap.SetTile(backgroundTilemap.WorldToCell(new Vector3Int(x, y, 0)), toInstantiate);
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
            for (int x = startPosCol - 1; x < endPosCol + 1; x++)
            {
                for (int y = startPosRow - 1; y < endPosRow + 1; y++)
                {
                    Tile toInstantiate;
                    //If we are in the background
                    if (x == startPosCol - 1 || x == endPosCol || y == startPosRow - 1 || y == endPosRow)
                    {

                        toInstantiate = backgroundTiles[Random.Range(0, backgroundTiles.Length)];
                        backgroundTilemap.SetTile(backgroundTilemap.WorldToCell(new Vector3Int(x, y, 0)), toInstantiate);
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

    //Returns a valid random position inside a column
    //Selects only available positions and returns a random one
    private Vector3Int RandomPosition(int col)
    {
        int row = Random.Range(startPosRow, endPosRow);
        col += startPosCol;
        Tilemap[] tilemaps = map.GetComponentsInChildren<Tilemap>();
        List<Vector3> availablePositions = CheckPositions(col, tilemaps);
        int a = availablePositions.Count;
        Vector3Int result = mapTilemap.WorldToCell(availablePositions[Random.Range(0, availablePositions.Count)]);

        return result;
    }

    private List<Vector3> CheckPositions(int col, Tilemap[] tileMaps)
    {
        List<Vector3> positions = new List<Vector3>();
        for (int i = startPosRow; i < endPosRow; i++)
        {
            bool availablePos = true;
            foreach (var tilemap in tileMaps)
            {
                if (tilemap.name == "Ground" || tilemap.name == "Background")
                    continue;
                if (tilemap.GetTile(new Vector3Int(col, i, 0)))
                {
                    availablePos = false;
                    break;
                }
            }
            if (availablePos)
                positions.Add(new Vector3(col, i, 0));
        }
        return positions;
    }

    //Checks if in all tilemaps given this tile has already been set. If that is the case, it is deleted.
    //private void UpdateTile(Tilemap[] allTilemaps, Vector3 position)
    //{
    //    foreach (var tilemap in allTilemaps)
    //    {
    //        if (tilemap.name == "Ground" || tilemap.name == "Background" || tilemap.name == "Walls")
    //            continue;
    //        if (tilemap.GetTile(tilemap.WorldToCell(position)))
    //        {
    //            tilemap.SetTile(tilemap.WorldToCell(position), null);
    //        }
    //    }
    //}

    private void LayoutObjectAtRandom(Tilemap tilemap, Tile[] tileArray, int minimum, int maximum)
    {
        for (int j = 0; j < columns; j++)
        {
            int objectCount = Random.Range(minimum, maximum + 1);
            for (int i = 0; i < objectCount; i++)
            {
                Vector3Int randomPosition = RandomPosition(j);
                tilemap.SetTile(randomPosition, tileArray[Random.Range(0, tileArray.Length)]);
            }
        }
    }

    private void LayoutObjectUniformly(Tilemap tilemap, Tile[] tileArray)
    {
        if (isCentered)
        {
            int colPos = startPosCol % 2 == 0 ? 1 : 0;
            int rowPos = startPosRow % 2 == 0 ? 1 : 0;
            for (int i = startPosCol; i < endPosCol; i++)
            {
                for (int j = startPosRow; j < endPosRow; j++)
                {
                    if (Mathf.Abs(i % 2) == colPos && Mathf.Abs(j % 2) == rowPos)
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), tileArray[Random.Range(0, tileArray.Length)]);
                    }
                }
            }
        }
        else
        {
            for (int i = startPosCol; i < endPosCol; i++)
            {
                for (int j = startPosRow; j < endPosRow; j++)
                {
                    if (i % 2 == 1 && j % 2 == 1)
                    {
                        tilemap.SetTile(new Vector3Int(i, j, 0), tileArray[Random.Range(0, tileArray.Length)]);
                    }
                }
            }
        }

    }

    private void GeneratePowerups()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Vector3 pos = new Vector3(i + startPosCol, j + startPosRow);
                Vector3 posInCell = pos + new Vector3(0.5f, 0.5f, 0f);
                if (destroyableWallsTilemap.GetTile(mapTilemap.WorldToCell(posInCell)))
                {
                    //50% of spawning a powerup
                    int probability = Random.Range(0, 3);
                    if (probability == 0)
                    {
                        GameObject instance = Instantiate<GameObject>(pickUps[Random.Range(0, pickUps.Length)], posInCell, Quaternion.identity);
                        SetParentPowerUp(instance);
                    }
                }
            }
        }
    }

    private void SetParentPowerUp(GameObject mypowerup)
    {
        if (mypowerup.tag == "Boost Speed")
        {
            mypowerup.transform.SetParent(boostSpeed.transform);
        }
        else if (mypowerup.tag == "Boost Inventory Bombs")
        {
            mypowerup.transform.SetParent(bombIncreasement.transform);
        }
        else if (mypowerup.tag == "Boost Explosion")
        {
            mypowerup.transform.SetParent(boostExplosion.transform);
        }
    }

    private void InitialiseGameObjects()
    {
        //Creating the map holder parent which contains the grid and a base tilemap.
        map = new GameObject("Map");
        map.AddComponent<Grid>();
        mapTilemap = map.AddComponent<Tilemap>();

        //Creating Gameobjects and setting their layers/tags
        background = new GameObject("Background")
        {
            tag = "Background",
            layer = LayerMask.NameToLayer("Background")
        };

        floor = new GameObject("Ground")
        {
            tag = "Ground",
            layer = LayerMask.NameToLayer("Ground")
        };

        walls = new GameObject("Walls")
        {
            tag = "Walls",
            layer = LayerMask.NameToLayer("Walls")
        };

        destroyableWalls = new GameObject("Destroyable Walls")
        {
            tag = "Destroyable Walls",
            layer = LayerMask.NameToLayer("Destroyable Walls")
        };

        powerups = new GameObject("Power Ups")
        {
            tag = "Power Up",
            layer = LayerMask.NameToLayer("Power Ups")
        };

        boostSpeed = new GameObject("Boost Speed")
        {
            tag = "Boost Speed",
            layer = LayerMask.NameToLayer("Power Ups")
        };

        bombIncreasement = new GameObject("Bomb Increasement")
        {
            tag = "Boost Inventory Bombs",
            layer = LayerMask.NameToLayer("Power Ups")
        };

        boostExplosion = new GameObject("Boost Explosion")
        {
            tag = "Boost Explosion",
            layer = LayerMask.NameToLayer("Power Ups")
        };

        //Setting up parents
        background.transform.SetParent(map.transform);
        floor.transform.SetParent(map.transform);
        walls.transform.SetParent(map.transform);
        destroyableWalls.transform.SetParent(map.transform);
        powerups.transform.SetParent(map.transform);
        boostSpeed.transform.SetParent(powerups.transform);
        bombIncreasement.transform.SetParent(powerups.transform);
        boostExplosion.transform.SetParent(powerups.transform);


        //Adding the tilemap and tilemapRenderer components and setting their sorting layer.
        backgroundTilemap = background.AddComponent<Tilemap>();
        TilemapRenderer backgroundTilemapRenderer = background.AddComponent<TilemapRenderer>();
        backgroundTilemapRenderer.sortingLayerName = "Background";

        floorTilemap = floor.AddComponent<Tilemap>();
        TilemapRenderer floorTilemapRenderer = floor.AddComponent<TilemapRenderer>();
        floorTilemapRenderer.sortingLayerName = "Ground";

        wallsTilemap = walls.AddComponent<Tilemap>();
        TilemapRenderer wallsTilemapRenderer = walls.AddComponent<TilemapRenderer>();
        wallsTilemapRenderer.sortingLayerName = "Walls";

        destroyableWallsTilemap = destroyableWalls.AddComponent<Tilemap>();
        TilemapRenderer destroyableWallsTilemapRenderer = destroyableWalls.AddComponent<TilemapRenderer>();
        destroyableWallsTilemapRenderer.sortingLayerName = "Destroyable Walls";
    }

    void InitialisePhysics()
    {
        //Adding physics
        Rigidbody2D rb2D = background.AddComponent<Rigidbody2D>();
        rb2D.bodyType = RigidbodyType2D.Static;
        TilemapCollider2D tilemapCollider2D = background.AddComponent<TilemapCollider2D>();
        tilemapCollider2D.usedByComposite = true;
        background.AddComponent<CompositeCollider2D>();

        Rigidbody2D rbWalls = walls.AddComponent<Rigidbody2D>();
        rbWalls.bodyType = RigidbodyType2D.Static;
        TilemapCollider2D wallsTilemapCollider2d = walls.AddComponent<TilemapCollider2D>();
        wallsTilemapCollider2d.usedByComposite = true;
        walls.AddComponent<CompositeCollider2D>();

        Rigidbody2D rbDestroyableWalls = destroyableWalls.AddComponent<Rigidbody2D>();
        rbDestroyableWalls.bodyType = RigidbodyType2D.Static;
        TilemapCollider2D destroyableWallsTilemapCollider2d = destroyableWalls.AddComponent<TilemapCollider2D>();
        destroyableWallsTilemapCollider2d.usedByComposite = true;
        destroyableWalls.AddComponent<CompositeCollider2D>();
    }

    private void InitialiseVariables()
    {
        //Initialise range
        wallCount = new Count(0, rows / 2);
        destroyableWallCount = new Count(1, rows / 2);
        bool oddRowNumber, oddColNumber;
        oddRowNumber = rows % 2 == 1 ? true : false;
        oddColNumber = columns % 2 == 1 ? true : false;
        //Initialise variables
        if (isCentered)
        {
            if (!oddColNumber)
            {
                startPosCol = (-columns / 2);
                endPosCol = (columns / 2);
            }
            else
            {
                startPosCol = (-columns / 2) - 1;
                endPosCol = (columns / 2);
            }

            if (!oddRowNumber)
            {
                startPosRow = (-rows / 2);
                endPosRow = (rows / 2);
            }
            else
            {
                startPosRow = (-rows / 2) - 1;
                endPosRow = (rows / 2);
            }

        }
        else
        {
            startPosCol = 0;
            endPosCol = columns;

            startPosRow = 0;
            endPosRow = rows;
        }
    }

    private void InitialisePlayersSpawnPosition()
    {
        GameObject spawnPos = new GameObject("Spawn Positions");
        spawnPos.transform.parent = map.transform;

        GameObject players = new GameObject("Players Spawn Positions");
        players.transform.parent = spawnPos.transform;

        GameObject enemies = new GameObject("Enemies Spawn Positions");
        enemies.transform.parent = spawnPos.transform;
        if (GameplayManager.instance.players.Length < 3)
        {

            GameObject player1 = new GameObject("Player 1");
            player1.transform.parent = players.transform;
            player1.transform.position = new Vector3(startPosCol + 0.5f, startPosRow + 0.5f, 0f);

            GameObject player2 = new GameObject("Player 2");
            player2.transform.parent = players.transform;
            player2.transform.position = new Vector3(endPosCol - 0.5f, endPosRow - 0.5f, 0f);

            GameplayManager.instance.players[0].spawnPoint = player1.transform;
            GameplayManager.instance.players[1].spawnPoint = player2.transform;

        }
        else
        {
            Debug.Log("Only 2 players are supported");
        }


    }

    private void Initialise()
    {
        InitialiseGameObjects();
        InitialiseVariables();
    }

    public void SetupScene(int level)
    {
        Initialise();
        GenerateBasicMap();
        InitialisePlayersSpawnPosition();
        LayoutObjectUniformly(wallsTilemap, wallTiles);
        LayoutObjectAtRandom(destroyableWallsTilemap, destroyableWallTiles, destroyableWallCount.minimum, destroyableWallCount.maximum);
        GeneratePowerups();
        InitialisePhysics();

    }

}
