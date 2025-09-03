using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Shard = System.String;


/// <summary> The audio manager. </summary>
public class AudioExec : MonoBehaviour
{
    #region EXCEPTIONS

    /// <summary> Something went wrong loading an audio file. </summary>
    public class AudioLoadException : Exception
    {}

    /// <summary> A playlist is empty so cannot be played. </summary>
    public class EmptyPlaylistException : Exception
    {}

    #endregion


    public AudioSource audioSource;

    /// <summary> Currently playing track (different to selected). `null` if no track is active. </summary>
    public Track activeTrack;

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
        this.audioSource.clip = clip;
        this.audioSource.volume = volume;
        this.audioSource.Play();
        return this.audioSource;
    }

    #endregion


    #region START PLAYBACK

    private void PlayCurrent()
    {
        var clip = LoadClip(this.activeTrack.shard);
        PlayClip(clip);
    }

    public void PlayNext()
    {
        ClearCurrent();

        if (this.trackQueue.Count > 0) {
            this.activeTrack = this.trackQueue[0];
            this.trackQueue.RemoveAt(0);
            PlayCurrent();
        }
        else {
            this.activeTrack = null;
        }
    }

    public void PlayNow(Track track)
    {
        ClearCurrent();

        this.activeTrack = track;
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
        this.trackQueue = playlist.tracks.ToList();
        PlayNext();
    }

    #endregion

    
    #region MOVE PLAYBACK

    public void Pause()
    {
        this.audioSource.Pause();
        this.isPaused = true;
    }

    public void UnPause()
    {
        if (this.activeTrack is null && this.trackQueue.Count > 0) {
            PlayNext();
        } else {
            this.audioSource.UnPause();
        }
        
        this.isPaused = false;
    }

    public void TogglePause()
    {
        if (this.isPaused) {
            UnPause();
        } else {
            Pause();
        }
    }

    public void Restart()
        => this.audioSource.time = 0;

    public void Seek(float to)
        => this.audioSource.time = to;

    public void Shift(float by)
        => this.audioSource.time += by;

    #endregion


    #region END PLAYBACK

    public void ClearCurrent()
    {
        this.audioSource.Stop();
        this.activeTrack = null;
    }

    public void ClearQueue()
    {
        this.trackQueue.Clear();
    }

    #endregion


    #region QUEUE PLAYBACK

    public void AddToQueue(Track track)
    {
        this.trackQueue.Add(track);
    }

    #endregion
}
