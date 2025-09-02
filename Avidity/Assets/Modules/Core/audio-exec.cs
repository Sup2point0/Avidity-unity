using System;
using System.Collections.Generic;

using UnityEngine;


public class AudioExec : MonoBehaviour
{
    private AudioSource source;

    public Track currentTrack;

    public bool isPaused;

    public float timeElapsed;

    public List<Track> trackQueue = new();


    #region UNITY

    void Awake()
    {
        Exec.Audio = this;

        source = new GameObject().AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
        DontDestroyOnLoad(source.gameObject);
    }

    #endregion


    #region INTERNAL

    private AudioSource PlaySound(AudioClip clip, float volume = 1.0f)
    {
        if (source is null) {
            throw new NullReferenceException("Audio Source has not been created!");
        }

        source.clip = clip;
        source.volume = volume;
        source.Play();
        return source;
    }

    private AudioClip LoadCip(string filename)
    {
        
    }

    #endregion


    #region START PLAYBACK

    public void PlayCurrent()
    {}

    public void PlayTrack(Track track)
    {}

    public void PlayNext()
    {}

    public void PlayList()
    {}

    #endregion

    
    #region MOVE PLAYBACK

    public void Restart()
    {}

    public void Seek(float to)
    {}

    public void Shift(float by)
    {}

    #endregion
}
