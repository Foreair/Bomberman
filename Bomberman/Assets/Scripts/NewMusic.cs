using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMusic : MonoBehaviour {

    public AudioClip newTrack;
    private void Start()
    {
        SoundManager.instance.PlayMusicClip(newTrack);
    }
}
