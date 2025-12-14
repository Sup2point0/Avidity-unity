using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class PlaybackInfoScript : Bases.InterfaceController
{
    public Label trackName;
    public Label artistName;


    void OnEnable()
    {
        this.trackName = this.ui.Q<Label>("track-name");
        this.artistName = this.ui.Q<Label>("artist-name");
    }

    void Start()
    {
        Exec.Audio.onTrackPlayed += OnTrackPlayed;
        Exec.Audio.onTrackCleared += OnTrackCleared;
    }


    void OnTrackPlayed()
    {
        this.trackName.text = Exec.Audio.activeTrack.name;
        this.artistName.text = Artist.DisplayNames(Exec.Audio.activeTrack.artists);
    }

    void OnTrackCleared()
    {
        this.trackName.text = "No Track Selected";
        this.artistName.text = "";
    }
}
