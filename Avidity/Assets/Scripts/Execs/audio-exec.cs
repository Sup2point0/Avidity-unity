using System;
using System.Collections.Generic;

using UnityEngine;

using Shard = System.String;


public class AudioExec : MonoBehaviour
{
    public class AudioLoadException : Exception
    {}


    public AudioSource audioSource;

    public Track currentTrack;

    public bool isPaused;

    public float timeElapsed;

    public List<Track> trackQueue = new();


    #region UNITY

    void Awake()
    {
        Exec.Audio = this;
    }

    #endregion


    #region INTERNAL

    private AudioSource PlayClip(AudioClip clip, float volume = 1.0f)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        return audioSource;
    }

    private AudioClip LoadClip(Shard shard)
    {
        var clip = Resources.Load<AudioClip>($"Tracks/{shard}");

        if (clip is null) {
            throw new AudioLoadException();
        }

        return clip;
    }

    #endregion


    #region START PLAYBACK

    public void PlayCurrent()
    {}

    public void PlayTrack(Track track)
    {
        var clip = LoadClip(track.shard);
        PlayClip(clip);
    }

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
