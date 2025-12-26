using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class SelectorPaneController : Bases.InterfaceController
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
        

        this.trackName.text  = track.DisplayName();
        this.artistName.text = Artist.DisplayNames(track.artists);
        this.albumName.text  = track.album?.ToString() ?? "None";

        this.trackDuration.text = track.DisplayDuration();
        this.trackPlays.text    = track.totalPlays.ToString();
        this.trackShard.text    = track.shard ?? "None Set";
    }
}
