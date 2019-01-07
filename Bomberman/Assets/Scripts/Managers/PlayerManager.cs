using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerManager {

    public Color playerColor;                               //Color of the player

    [HideInInspector] public Transform spawnPoint;          //Spawn location of this player
    [HideInInspector] public int playerNumber;              //Player id number
    [HideInInspector] public GameObject instance;           //Instance of the Player associated with this manager.
    [HideInInspector] public int wins;                      //Current number of wins for this player.
    [HideInInspector] public int lifes;                     //Current number of lifes for this player.
    [HideInInspector] public string coloredPlayerText;      //string for the UI to show player's color

    private PlayerController playerController;
    private PlayerInput playerInput;
    private PlayerData playerData;

    public void Setup()
    {
        wins = 0;
        lifes = GameplayManager.instance.lifes;

        playerController = instance.GetComponent<PlayerController>();
        playerInput = playerController.playerInput;
        playerData = playerController.playerData;

        playerInput.PlayerNumber = playerNumber;
        playerInput.InitializeInput();
        playerData.InitializeData();

        coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNumber + "</color>";
        SpriteRenderer[] renderers = instance.GetComponentsInChildren<SpriteRenderer>();

        foreach (var item in renderers)
        {
            item.material.color = playerColor;
        }
    }

    public void DisableControl()
    {
        playerController.enabled = false;
    }

    public void EnableControl()
    {
        playerController.enabled = true;
    }

    public void Reset()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;

        playerData.InitializeData();
        playerInput.InitializeInput();

        //Checkear si esto es útil y por qué
        instance.SetActive(false);
        instance.SetActive(true);
    }

}
