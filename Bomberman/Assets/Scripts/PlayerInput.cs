using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInput {

    [HideInInspector] public int PlayerNumber;
    public string horizontalAxis;
    public string verticalAxis;
    public string bombButton;
    public static string pauseButton = "Pause";

    public void InitializeInput()
    {
        horizontalAxis = "Horizontal_P" + PlayerNumber;
        verticalAxis = "Vertical_P" + PlayerNumber;
        bombButton = "Bomb_P" + PlayerNumber;
    }
}
