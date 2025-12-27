using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

using Avidity;


/// <summary> The audio manager. </summary>
public class AudioExecutive : MonoBehaviour
{
#region EXCEPTIONS

    /// <summary> Something went wrong loading an audio file. </summary>
    public class AudioLoadException : Bases.DisplayedException {
        public AudioLoadException(string message) : base("Audio Load Error", message) {}
    }

    /// <summary> A playlist is empty so cannot be played. </summary>
    public class EmptyPlaylistException : Bases.DisplayedException {
        public EmptyPlaylistException() : base("Playlist is empty") {}
    }

#endregion


#region DELEGATES

    public Action onTrackPlayed;
    public Action onPlaybackUpdated;
    public Action onPlaybackUnpaused;
    public Action onPlaybackPaused;
    public Action onTrackCleared;
    public Action onQueueUpdated;

#endregion


    public AudioSource audioSource;

    /// <summary> Currently playing track (different to selected). `null` if no track is active. </summary>
    public Track activeTrack;

    /// <summary> Is the audio paused? </summary>
    public bool isPaused;

    /// <summary> Playback position in seconds. </summary>
    public float timeElapsed;

    /// <summary> The queue of tracks and their temporary uniquifier IDs to play next. </summary>
    public List<(uint, Track)> queuedTracks = new();
    /* NOTE: won't show in the editor, cuz Unity doesn't like serialising tuples... */


    private uint qid_counter;


#region UNITY

    void Awake()
    {
        Exec.Audio = this;
    }

#endregion


#region START PLAYBACK

    private void PlayCurrent()
    {
        async void Play() {
            var clip = await LoadClipAsync(this.activeTrack);
            PlayClip(clip);
        }

        Play();
    }

    public void PlayNext()
    {
        ClearCurrent();

        if (this.queuedTracks.Count > 0) {
            this.activeTrack = this.queuedTracks[0].Item2;
            UnqueueFirst();
            PlayCurrent();
        }
        else {
            this.activeTrack = null;
        }
    }

    public void PlayNow(Track track, uint? qid = null)
    {
        ClearCurrent();

        this.activeTrack = track;

        if (qid.HasValue) {
            UnqueueTrack(qid.Value);
        }

        PlayCurrent();
    }

    public void PlayNow(Playlist playlist)
    {
        if (playlist.tracks.Count == 0) {
            throw new EmptyPlaylistException();
        }

        ClearCurrent();
        ClearQueue();

        this.queuedTracks = playlist.tracks.Select((value, index) => ((uint) index, value)).ToList();
        PlayNext();
    }

#endregion

    
#region MOVE PLAYBACK

    public void Pause()
    {
        this.audioSource.Pause();
        this.isPaused = true;

        this.onPlaybackUpdated?.Invoke();
        this.onPlaybackPaused?.Invoke();
    }

    public void Unpause()
    {
        if (this.activeTrack is null && this.queuedTracks.Count > 0) {
            PlayNext();
        } else {
            this.audioSource.UnPause();
        }
        
        this.isPaused = false;

        this.onPlaybackUpdated?.Invoke();
        this.onPlaybackUnpaused?.Invoke();
    }

    public void TogglePause()
    {
        if (this.isPaused) {
            Unpause();
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

        this.onTrackCleared?.Invoke();
    }

#endregion


#region QUEUE PLAYBACK

    public void QueueTrack(Track track)
    {
        this.queuedTracks.Add((++this.qid_counter, track));
        this.onQueueUpdated?.Invoke();
    }

    /// <summary> Remove a track from the playback queue by its queue ID. </summary>
    /// <param name="qid">The assigned queue ID of the track.</param>
    public void UnqueueTrack(uint qid)
    {
        this.queuedTracks.RemoveAll( entry => entry.Item1 == qid );
        this.onQueueUpdated?.Invoke();
    }

    public void UnqueueFirst()
    {
        this.queuedTracks.RemoveAt(0);
        this.onQueueUpdated?.Invoke();
    }

    public void ClearQueue()
    {
        this.queuedTracks.Clear();
        this.qid_counter = 0;
        this.onQueueUpdated?.Invoke();
    }

#endregion


#region LOAD AUDIO

    public async Awaitable<AudioClip> LoadClipAsync(Track track)
    {
        if (track.shard is null) throw new AudioLoadException("Cannot play a track with no shard set");

        var url = $"file://C:/Users/sup/Desktop/assets/sounds/{track.artists[0].shard}/{track.shard}.mp3";
        using var request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN);

        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError) {
            throw new AudioLoadException("Failed to find file path of track");
        } else {
            var clip = DownloadHandlerAudioClip.GetContent(request)
                ?? throw new AudioLoadException("Failed to load audio clip for track");

            track.duration = clip.length;

            return clip;
        }
    }

#endregion


#region INTERNAL

    private AudioSource PlayClip(AudioClip clip, float volume = 1.0f)
    {
        this.audioSource.clip = clip;
        this.audioSource.volume = volume;
        this.audioSource.Play();
        this.isPaused = false;

        this.onTrackPlayed?.Invoke();

        return this.audioSource;
    }

#endregion
}
