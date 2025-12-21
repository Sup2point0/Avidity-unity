using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class SelectorScript : Bases.InterfaceController
{
    public Label trackName;
    public Label artistName;
    public Label albumName;

    public Label trackDuration;
    public Label trackPlays;
    public Label trackShard;


    void OnEnable()
    {
        var root = this.ui.Q<VisualElement>("selector");

        this.trackName  = root.Q<Label>("track-name");
        this.artistName = root.Q<Label>("artist-name");
        this.albumName  = root.Q<Label>("album-name");

        this.trackDuration = root.Q<Label>("track-duration");
        this.trackPlays    = root.Q<Label>("track-plays");
        this.trackShard    = root.Q<Label>("track-shard");
    }

    void Start()
    {
        Exec.Scene.onTrackSelected += OnTrackSelected;
    }


    void OnTrackSelected()
    {
        var track = Exec.Scene.selectedTrack;
        var file = Resources.Load<AudioClip>($"Tracks/{track.shard}");

        this.trackName.text  = track.name;
        this.artistName.text = Artist.DisplayNames(track.artists);
        this.albumName.text  = track.album?.name ?? "";

        this.trackDuration.text = file? $"{file.length} s" : "unknown";
        this.trackPlays.text    = track.totalPlays.ToString();
        this.trackShard.text    = track.shard;
    }
}
