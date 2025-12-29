using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class PlaybackInfoScript : Bases.InterfaceController
{
    public Label trackName;
    public Label artistName;


    void OnEnable()
    {
        var root = this.ui.Q<VisualElement>("playback");
        this.trackName = root.Q<Label>("track-name");
        this.artistName = root.Q<Label>("artist-name");
    }

    void Start()
    {
        Exec.Audio.onTrackPlayed += OnTrackPlayed;
        Exec.Audio.onPlaybackCleared += OnTrackCleared;
    }


    void OnTrackPlayed()
    {
        this.trackName.text  = Exec.Audio.activeTrack.DisplayName();
        this.artistName.text = Artist.DisplayNames(Exec.Audio.activeTrack.artists);
    }

    void OnTrackCleared()
    {
        this.trackName.text  = "No Track Selected";
        this.artistName.text = "";
    }
}
