using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Shard = System.String;


/// <summary> An audio manager. </summary>
public class AudioExec : MonoBehaviour
{
    /// <summary> Something went wrong loading an audio file. </summary>
    public class AudioLoadException : Exception
    {}

    /// <summary> A playlist is empty so cannot be played. </summary>
    public class EmptyPlaylistException : Exception
    {}


    public AudioSource audioSource;

    /// <summary> Currently active track. `null` if no track is active. </summary>
    public Track currentTrack;

    /// <summary> Is the audio paused? </summary>
    public bool isPaused;

    /// <summary> Playback position in seconds. </summary>
    public float timeElapsed;

    /// <summary> The queue of tracks to play next. </summary>
    public List<Track> trackQueue = new();


    #region UNITY

    void Awake()
    {
        Exec.Audio = this;
    }

    #endregion


    #region INTERNAL

    private AudioClip LoadClip(Shard shard)
    {
        var clip = Resources.Load<AudioClip>($"Tracks/{shard}");

        if (clip is null) {
            throw new AudioLoadException();
        }

        return clip;
    }

    private AudioSource PlayClip(AudioClip clip, float volume = 1.0f)
    {
        Debug.Log($"clip = {clip}");
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        return audioSource;
    }

    #endregion


    #region START PLAYBACK

    private void PlayCurrent()
    {
        var clip = LoadClip(currentTrack.shard);
        PlayClip(clip);
    }

    public void PlayNext()
    {
        ClearCurrent();

        if (trackQueue.Count > 0) {
            currentTrack = trackQueue[0];
            trackQueue.RemoveAt(0);
            PlayCurrent();
        }
        else {
            currentTrack = null;
        }
    }

    public void PlayNow(Track track)
    {
        ClearCurrent();

        currentTrack = track;
        PlayCurrent();
    }

    public void PlayNow(Playlist playlist)
    {
        if (0 >= playlist.tracks.Count) {
            throw new EmptyPlaylistException();
        }

        ClearCurrent();
        ClearQueue();

        /* Create a shallow copy so that modifying queue does not corrupt playlist */
        trackQueue = playlist.tracks.ToList();
        PlayNext();
    }

    #endregion

    
    #region MOVE PLAYBACK

    public void Pause()
    {
        audioSource.Pause();
        isPaused = true;
    }

    public void UnPause()
    {
        if (currentTrack is null && trackQueue.Count > 0) {
            PlayNext();
        } else {
            audioSource.UnPause();
        }
        
        isPaused = false;
    }

    public void TogglePause()
    {
        if (isPaused) {
            UnPause();
        } else {
            Pause();
        }
    }

    public void Restart()
        => audioSource.time = 0;

    public void Seek(float to)
        => audioSource.time = to;

    public void Shift(float by)
        => audioSource.time += by;

    #endregion


    #region END PLAYBACK

    public void ClearCurrent()
    {
        audioSource.Stop();
        currentTrack = null;
    }

    public void ClearQueue()
    {
        trackQueue.Clear();
    }

    #endregion


    #region QUEUE PLAYBACK

    public void AddToQueue(Track track)
    {
        trackQueue.Add(track);
    }

    #endregion
}
