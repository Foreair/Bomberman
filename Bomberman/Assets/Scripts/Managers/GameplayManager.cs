using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameplayManager : MonoBehaviour {

    public static GameplayManager instance = null;

    public int roundsToWin = 2;                 // The number of rounds a single player has to win to win the game.
    public float startDelay = 3f;               // The delay between the start of RoundStarting and RoundPlaying phases.
    public float endDelay = 3f;                 // The delay between the end of RoundPlaying and RoundEnding phases.
    public GameObject playerPrefab;             // Reference to the prefab the players will control.
    public PlayerManager[] players;             // A collection of managers for enabling and disabling different aspects of the players.

    private int level;                          // Which level the game is currently on.
    private int roundNumber;                    // Which round the game is currently on.
    private Text messageText;               // Reference to the overlay Text to display winning text, etc.
    private WaitForSeconds startWait;           // Used to have a delay whilst the round starts.
    private WaitForSeconds endWait;             // Used to have a delay whilst the round or game ends.
    private PlayerManager roundWinner;          // Reference to the winner of the current round.  Used to make an announcement of who won.
    private PlayerManager gameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        if (SceneManager.GetActiveScene().name == "2P Level1")
            LoadMultiplayer();
    }
    public void LoadSinglePlayer()
    {
        SceneManager.LoadScene("1P Level1");
    }

    public void LoadMultiplayer()
    {
        StartCoroutine(LoadMP());
    }

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

    private void StartMultiplayer()
    {
        // Create the delays so they only have to be made once.
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);
        messageText = GameObject.Find("Text").GetComponent<Text>();

        //GetSpawnTransforms();
        SpawnMultiplayerMode();

        // Once the tanks have been created and the camera is using them as targets, start the game.
        StartCoroutine(GameLoopMP());
    }

    //private void GetSpawnTransforms()
    //{
    //    for (int i = 0; i < players.Length; i++)
    //    {
    //        string name = "SpawnPlayer" + (i + 1);
    //        players[i].spawnPoint = GameObject.Find(name).transform;
    //    }
    //}

    private void SpawnMultiplayerMode()
    {
        //For all players...
        for (int i = 0; i < players.Length; i++)
        {
            // ... create them, set their player number and references needed for control.
            players[i].instance = Instantiate(playerPrefab, players[i].spawnPoint.position, players[i].spawnPoint.rotation) as GameObject;
            players[i].playerNumber = i + 1;
            players[i].Initialize();
        }
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoopMP()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundStarting());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEnding());

        // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
        if (gameWinner != null)
        {
            // If there is a game winner, restart the level.
            LoadMultiplayer();
        }
        else
        {
            // If there isn't a winner yet, restart this coroutine so the loop continues.
            // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
            StartCoroutine(GameLoopMP());
        }
    }

    private IEnumerator RoundStarting()
    {
        // As soon as the round starts reset the players and/or enemies and make sure they can't move.
        ResetTransforms();
        DisableControl();

        // Increment the round number and display text showing the players what round it is.
        roundNumber++;
        messageText.text = "ROUND " + roundNumber;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return startWait;
    }


    private IEnumerator RoundPlaying()
    {
        // As soon as the round begins playing let the players control the tanks.
        EnableControl();

        // Clear the text from the screen.
        messageText.text = string.Empty;

        // While there is not one tank left...
        while (!OnePlayerLeft())
        {
            // ... return on the next frame.
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        // Stop tanks from moving.
        DisableControl();

        // Clear the winner from the previous round.
        roundWinner = null;

        // See if there is a winner now the round is over.
        roundWinner = GetRoundWinner();

        // If there is a winner, increment their score.
        if (roundWinner != null)
            roundWinner.wins++;

        // Now the winner's score has been incremented, see if someone has one the game.
        gameWinner = GetGameWinner();

        // Get a message based on the scores and whether or not there is a game winner and display it.
        string message = EndMessage();
        messageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return endWait;
    }

    // This is used to check if there is one or fewer tanks remaining and thus the round should end.
    private bool OnePlayerLeft()
    {
        // Start the count of players left at zero.
        int numPlayersLeft = 0;

        // Go through all the tanks...
        for (int i = 0; i < players.Length; i++)
        {
            // ... and if they are active, increment the counter.
            if (players[i].instance.activeSelf)
                numPlayersLeft++;
        }

        // If there are one or fewer payers remaining return true, otherwise return false.
        return numPlayersLeft <= 1;
    }


    // This function is to find out if there is a winner of the round.
    // This function is called with the assumption that 1 or fewer tanks are currently active.
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

        // If no tanks have enough rounds to win, return null.
        return null;
    }


    // Returns a string message to display at the end of each round.
    private string EndMessage()
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
            message = gameWinner.coloredPlayerText + " WINS THE GAME!";

        return message;
    }


    // This function is used to turn all the players and/or enemies back on and reset their positions and properties.
    private void ResetTransforms()
    {
        foreach (var player in players)
        {
            player.Reset();
        }
    }

    private void DisableControl()
    {
        foreach (var player in players)
        {
            player.DisableControl();
        }
    }

    private void EnableControl()
    {
        foreach (var player in players)
        {
            player.EnableControl();
        }
    }

}
