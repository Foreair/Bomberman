using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource sfxSource, musicSource;
    
    //singleton
    public static SoundManager instance = null;

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle (AudioClip clip)
    {
        if (!checkClipViability(clip))
            return;

        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void RandomizeSfx(params AudioClip [] clips)
    {
        if (!checkClipsViability(clips))
            return;

        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        sfxSource.pitch = randomPitch;
        sfxSource.clip = clips[randomIndex];
        sfxSource.Play();
    }

    public void PlayMusicClip (AudioClip clip, float delay = 0.0f)
    {
        if (!checkClipViability(clip))
            return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.PlayDelayed(delay);
    }

    public void PlaySfxClip(AudioClip clip, float delay = 0.0f)
    {
        if (!checkClipViability(clip))
            return;

        sfxSource.Stop();
        sfxSource.clip = clip;
        sfxSource.PlayDelayed(delay);
    }

    private bool checkClipViability (AudioClip clip)
    {
        //If we dont have a clip
        if (clip == null)
            return false;
        //If the clip given is the same as the clip we are playing. Checked in both channels
        else if (musicSource.clip && clip.name == musicSource.clip.name || sfxSource.clip && clip.name == sfxSource.clip.name)
            return false;

        return true;
    }

    private bool checkClipsViability(AudioClip [] clips)
    {
        if (clips.Length == 0)
            return false;

        return true;
    }
}
