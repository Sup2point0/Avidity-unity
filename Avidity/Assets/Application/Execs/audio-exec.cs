using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

using Avidity;
using Avid = Avidity;


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
    public Action onPlaybackStarted;
    public Action onPlaybackPaused;
    public Action onPlaybackUnpaused;
    public Action onPlaybackFinished;
    public Action onPlaybackCleared;
    public Action onQueueUpdated;

    private Coroutine select_playing_track;
    private Coroutine watch_for_track_end;

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
        this.onPlaybackFinished += this.PlayNext;
        this.onPlaybackStarted += () => {
            if (Exec.Scene.selectedTrack is null) {
                Exec.Scene.SelectTrack(this.activeTrack);
            }
        };

        Exec.Audio = this;
    }

#endregion


#region START PLAYBACK

    /// <summary> Play the currently selected track. </summary>
    private void PlayCurrent()
    {
        if (this.watch_for_track_end != null) {
            StopCoroutine(this.watch_for_track_end);
        }
        if (this.select_playing_track != null) {
            StopCoroutine(this.select_playing_track);
        }

        PlayTrack(this.activeTrack);
        this.watch_for_track_end  = StartCoroutine(WatchForClipFinish());
        this.select_playing_track = StartCoroutine(WaitForClipStart());
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

        QueueTracks(playlist.tracks);
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

        this.onPlaybackCleared?.Invoke();
    }

#endregion


#region QUEUE PLAYBACK

    public void QueueTrack(Track track)
    {
        this.queuedTracks.Add((++this.qid_counter, track));
        this.onQueueUpdated?.Invoke();
    }

    public void QueueTracks(IEnumerable<Track> tracks)
    {
        if (this.queuedTracks.Count == 0) {
            // one day TODO: could be more efficient, maybe? Essentially want an Rc<Refcell<>>, I suppose.
            this.queuedTracks.Clear();
            this.queuedTracks.AddRange(
                tracks.Select((track, index) => ((uint) index, track)).ToList()
            );
        }
        else {
            this.queuedTracks.AddRange(
                tracks.Select((track, index) => (++this.qid_counter, track)).ToList()
            );
        }
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
        if (track.shard is null && track.audio_file is null) {
            throw new AudioLoadException("Cannot play a track with no shard set");
        }

        var urls = track.ResolveAudioSources();
        if (urls is null || urls.Count == 0) {
            throw new AudioLoadException("Failed to resolve possible track file sources");
        }

        int error_status = 0;

        foreach (var url in urls) {
            using var request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN);

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success) {
                if (error_status < 1) error_status = 1;
                continue;
            }

            var clip = DownloadHandlerAudioClip.GetContent(request);
            if (clip == null) {
                error_status = 2;
                continue;
            }

            track.duration = clip.length;
            return clip;
        }

        throw new AudioLoadException(
            error_status switch {
                0 => "Failed to find file path of track",
                1 => "Failed to access audio file of track",
                2 => "Failed to load audio clip for track",
                _ => "Something went wrong when trying to load audio",
            }
            + "; looked in: "
            + string.Join(", ", urls.Select(u => $"`{u}`"))
        );
    }

#endregion


#region INTERNAL

    private async void PlayTrack(Track track)
    {
        var clip = await LoadClipAsync(track);
        PlayClip(clip);
    }

    private AudioSource PlayClip(AudioClip clip, float volume = 1.0f)
    {
        this.audioSource.clip = clip;
        this.audioSource.volume = volume;
        this.audioSource.Play();
        this.isPaused = false;

        this.onTrackPlayed?.Invoke();

        return this.audioSource;
    }

    System.Collections.IEnumerator WaitForClipStart() {
        yield return new WaitWhile(() => !this.audioSource.isPlaying);
        this.onPlaybackStarted?.Invoke();
    }

    private System.Collections.IEnumerator WatchForClipFinish()
    {
        yield return new WaitWhile(() => !this.audioSource.isPlaying);
        yield return new WaitWhile(() => this.audioSource.isPlaying || this.isPaused);
        this.onPlaybackFinished?.Invoke();
    }

#endregion
}
