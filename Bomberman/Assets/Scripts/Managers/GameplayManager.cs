﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameplayManager : MonoBehaviour
{

    public static GameplayManager instance = null;
    private LevelManager levelManager;

    public int roundsToWin = 1;                         // The number of rounds a single player has to win to win the game.
    public float startDelay = 3f;                       // The delay between the start of RoundStarting and RoundPlaying phases.
    public float endDelay = 3f;                         // The delay between the end of RoundPlaying and RoundEnding phases.
    public GameObject playerPrefab;                     // Reference to the prefab the players will control.
    public GameObject enemyPrefab;                      // Reference to the prefab the players will control.
    public int lifes = 2;                               // Opportunities the player has before resetting level to 1.
    public Color player1Color = Color.cyan;             // Color of Player 1
    public Color player2Color = Color.yellow;           // Color of Player 2

    [HideInInspector] public PlayerManager[] players;   // A collection of managers for enabling and disabling different aspects of the players.
    [HideInInspector] public CreepManager[] enemies;    // A collection of managers for enabling and disabling different aspects of the players.
    [HideInInspector] public int enemiesNumber;         // Current number of enemies spawned on this level
    [HideInInspector] public GameObject map;            // Reference to the map. Each Map is represented by its grid containing different tilemaps.

    private int level = 1;                              // Which level the game is currently on.
    private int roundNumber;                            // Which round the game is currently on.
    private Text messageText;                           // Reference to the overlay Text to display winning text, etc.
    private WaitForSeconds startWait;                   // Used to have a delay whilst the round starts.
    private WaitForSeconds endWait;                     // Used to have a delay whilst the round or game ends.
    private PlayerManager roundWinner;                  // Reference to the winner of the current round.  Used to make an announcement of who won.
    private PlayerManager gameWinner;                   // Reference to the winner of the game.  Used to make an announcement of who won.
    private bool start;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        levelManager = GetComponent<LevelManager>();
        DontDestroyOnLoad(gameObject);

        start = true;

        if (SceneManager.GetActiveScene().name == "2P Level1")
            LoadMultiplayer();

        if (SceneManager.GetActiveScene().name == "1P Level1")
            LoadSinglePlayer();
    }

    private void InitializePlayerManagers(int number)
    {
        players = new PlayerManager[number];
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new PlayerManager();
        }
    }

    private void InitializeEnemyManagers(int number)
    {
        enemies = new CreepManager[number];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = new CreepManager();
        }
    }

    //Function Called to start the SinglePlayerMode
    //Initialises Players Managers and calls the coroutine LoadSP
    public void LoadSinglePlayer()
    {
        enemiesNumber = (int)Mathf.Log(level, 2f);
        if (enemiesNumber == 0)
            enemiesNumber = 1;
        InitializePlayerManagers(1);
        InitializeEnemyManagers(enemiesNumber);
        
        //Hardcoded colors
        players[0].playerColor = player1Color;

        //Starting Coroutine
        StartCoroutine(LoadSP());
    }

    //Function Called to start the MultiPlayerMode
    //Initialises Players Managers and calls the coroutine LoadMP
    public void LoadMultiplayer()
    {
        //Initializing PlayerManagers
        InitializePlayerManagers(2);

        //Hardcoded colors
        players[0].playerColor = player1Color;
        players[1].playerColor = player2Color;

        //Starting Coroutine
        StartCoroutine(LoadMP());
    }

    //Loads the scene and calls StartSinglePlayer
    IEnumerator LoadSP()
    {
        SceneManager.LoadScene("1P Level1");
        yield return null;
        StartSingleplayer();
    }

    //Loads the scene and calls StartMultiPlayer
    IEnumerator LoadMP()
    {
        SceneManager.LoadScene("2P Level1");
        yield return null;
        StartMultiplayer();
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    private void GenerateMap(int currentLevel)
    {
        levelManager.SetupScene(currentLevel);
    }

    //Creates the delays for the start and end of the rounds, gets the UI in-game component
    //Creates the map and spawns the players/enemies in their positions
    //and finally starts the game loop
    private void StartSingleplayer()
    {
        // Create the delays so they only have to be made once.
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);
        messageText = GameObject.Find("Text").GetComponent<Text>();

        //Creating map and generating spawnPositions
        GenerateMap(level);

        //Now that we have spawnpositions preset, spawn players and enemies
        SpawnPlayers();
        SpawnEnemies();

        // Once the players have been created, start the game.
        StartCoroutine(GameLoopSP());
    }

    //Creates the delays for the start and end of the rounds, gets the UI in-game component
    //Creates the map and spawns the players/enemies in their positions
    //and finally starts the game loop
    private void StartMultiplayer()
    {
        // Create the delays so they only have to be made once.
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);
        messageText = GameObject.Find("Text").GetComponent<Text>();

        GenerateMap(level);
        SpawnPlayers();

        // Once the players have been created, start the game.
        StartCoroutine(GameLoopMP());
    }

    private void SpawnPlayers()
    {
        //For all players...
        for (int i = 0; i < players.Length; i++)
        {
            // ... create them, set their player number and references needed for control.
            players[i].instance = Instantiate(playerPrefab, players[i].spawnPoint.position, players[i].spawnPoint.rotation) as GameObject;
            players[i].playerNumber = i + 1;
            players[i].lifes = lifes;
            players[i].Setup();
        }
    }

    private void SpawnEnemies()
    {
        //For all players...
        for (int i = 0; i < enemies.Length; i++)
        {
            // ... create them, set their player number and references needed for control.
            enemies[i].instance = Instantiate(enemyPrefab, enemies[i].spawnPoint.position, enemies[i].spawnPoint.rotation) as GameObject;
            enemies[i].Setup();
        }
    }

    private IEnumerator GameLoopSP()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundStartingSP());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundPlayingSP());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEndingSP());

        // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
        if (gameWinner != null)
        {
            // If the player has won...
            //...we move to the next level and recover our initial lifes
            UpdateVariables();
            LoadSinglePlayer();
        }
        else
        {
            //If the player has lost, we check if he still has lifes remaining
            if (players[0].lifes > 0)
                StartCoroutine(GameLoopSP());
            else
            {
                level = 0;
                UpdateVariables();
                LoadSinglePlayer();
            }
        }
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoopMP()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundStartingMP());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundPlayingMP());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEndingMP());

        // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
        if (gameWinner != null)
        {
            // If there is a game winner, update variables and move to the next level.
            UpdateVariables();
            LoadMultiplayer();
        }
        else
        {
            // If there isn't a winner yet, restart this coroutine so the loop continues.
            // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
            StartCoroutine(GameLoopMP());
        }
    }

    private IEnumerator RoundStartingSP()
    {
        ResetMap();
        ResetUnits();
        DisableControl();

        messageText.text = "LEVEL " + level;

        yield return startWait;
    }

    private IEnumerator RoundStartingMP()
    {
        // As soon as the round starts reset the players and/or enemies and make sure they can't move.
        ResetUnits();
        ResetMap();
        DisableControl();

        // Increment the round number and display text showing the players what round it is.
        roundNumber++;
        messageText.text = "ROUND " + roundNumber;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return startWait;
    }

    private IEnumerator RoundPlayingSP()
    {
        // As soon as the round begins playing let the players control the bombermans.
        EnableControl();

        // Clear the text from the screen.
        messageText.text = string.Empty;

        // While there is not one player left...
        while (!NoPlayersOrNoEnemiesLeft())
        {
            // ... return on the next frame.
            yield return null;
        }
    }

    private IEnumerator RoundPlayingMP()
    {
        // As soon as the round begins playing let the players control the bombermans.
        EnableControl();

        // Clear the text from the screen.
        messageText.text = string.Empty;

        // While there is not one player left...
        while (!OnePlayerLeft())
        {
            // ... return on the next frame.
            yield return null;
        }
    }

    private IEnumerator RoundEndingSP()
    {
        // Stop players from moving.
        DisableControl();

        // Clear the winner from the previous level.
        gameWinner = null;

        // See if there is a winner now the round is over.
        //We use GetRoundWinner because it checks if someone is still standing
        gameWinner = GetRoundWinner();

        //If the player has not won...
        if (gameWinner == null)
            players[0].lifes--;

        // Get a message based on the scores and whether or not there is a game winner and display it.
        string message = EndMessageSP();
        messageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return endWait;
    }

    private IEnumerator RoundEndingMP()
    {
        // Stop players from moving.
        DisableControl();

        // Clear the winner from the previous round.
        roundWinner = null;

        // See if there is a winner now the round is over.
        roundWinner = GetRoundWinner();

        // If there is a winner, increment their score.
        if (roundWinner != null)
            roundWinner.wins++;

        // Now the winner's score has been incremented, see if someone has won the game.
        gameWinner = GetGameWinner();

        // Get a message based on the scores and whether or not there is a game winner and display it.
        string message = EndMessageMP();
        messageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return endWait;
    }

    // This is used to check if there is one or fewer players remaining and thus the round should end.
    private bool OnePlayerLeft()
    {
        // Start the count of players left at zero.
        int numLeft = 0;

        // Go through all the players...
        for (int i = 0; i < players.Length; i++)
        {
            // ... and if they are active, increment the counter.
            if (players[i].instance.activeSelf)
                numLeft++;
        }

        // If there are one or fewer payers remaining return true, otherwise return false.
        return numLeft <= 1;
    }

    private bool NoPlayersOrNoEnemiesLeft()
    {
        int playersLeft = 0, enemiesLeft = 0;

        // Go through all the players...
        for (int i = 0; i < players.Length; i++)
        {
            // ... and if they are active, increment the counter.
            if (players[i].instance.activeSelf)
                playersLeft++;
        }

        if (playersLeft == 0)
            return true;

        // Go through all the players...
        for (int i = 0; i < enemies.Length; i++)
        {
            // ... and if they are active, increment the counter.
            if (enemies[i].instance.activeSelf)
                enemiesLeft++;
        }

        if (enemiesLeft == 0)
            return true;

        return false;

    }

    // This function is to find out if there is a winner of the round.
    // This function is called with the assumption that 1 or fewer players are currently active.
    private PlayerManager GetRoundWinner()
    {
        // Go through all the players...
        for (int i = 0; i < players.Length; i++)
        {
            // ... and if one of them is active, it is the winner so return it.
            if (players[i].instance.activeSelf)
                return players[i];
        }

        // If none of the players are active it is a draw so return null.
        return null;
    }


    // This function is to find out if there is a winner of the game.
    private PlayerManager GetGameWinner()
    {
        // Go through all the players...
        for (int i = 0; i < players.Length; i++)
        {
            // ... and if one of them has enough rounds to win the game, return it.
            if (players[i].wins == roundsToWin)
                return players[i];
        }

        // If no players have enough rounds to win, return null.
        return null;
    }


    private string EndMessageSP()
    {
        string message = "DEFEAT!";

        // Add some line breaks after the initial message.
        message += "\n\n\n\n";

        // Go through all the players and add each of their scores to the message.
        for (int i = 0; i < players.Length; i++)
        {
            message += players[i].coloredPlayerText + ": " + players[i].lifes + " LIFES\n";
        }

        if (gameWinner != null)
            message = "VICTORY!";

        return message;
    }

    // Returns a string message to display at the end of each round.
    private string EndMessageMP()
    {
        // By default when a round ends there are no winners so the default end message is a draw.
        string message = "DRAW!";

        // If there is a winner then change the message to reflect that.
        if (roundWinner != null)
            message = roundWinner.coloredPlayerText + " WINS THE ROUND!";

        // Add some line breaks after the initial message.
        message += "\n\n\n\n";

        // Go through all the players and add each of their scores to the message.
        for (int i = 0; i < players.Length; i++)
        {
            message += players[i].coloredPlayerText + ": " + players[i].wins + " WINS\n";
        }

        // If there is a game winner, change the entire message to reflect that.
        if (gameWinner != null)
            message = gameWinner.coloredPlayerText + " WINS!";

        return message;
    }


    // This function is used to turn all the players and/or enemies back on and reset their positions and properties.
    private void ResetUnits()
    {
        foreach (var player in players)
        {
            player.Reset();
        }

        foreach (var enemy in enemies)
        {
            enemy.Reset();
        }

    }

    private void ResetMap()
    {
        if (!start)
        {
            Destroy(map);
            GenerateMap(level);
        }
        else
        {
            start = false;
        }

    }

    private void DisableControl()
    {
        foreach (var player in players)
        {
            player.DisableControl();
        }

        foreach (var enemy in enemies)
        {
            enemy.DisableControl();
        }
    }

    private void EnableControl()
    {
        foreach (var player in players)
        {
            player.EnableControl();
        }

        foreach (var enemy in enemies)
        {
            enemy.EnableControl();
        }
    }

    private void UpdateVariables()
    {
        level++;
        roundNumber = 0;

    }
}
