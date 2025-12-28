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

    public Label trackSource;
    public Label trackShard;


    void OnEnable()
    {
        var root = this.ui.Q<VisualElement>("selector");

        this.trackName  = root.Q<Label>("track-name");
        this.artistName = root.Q<Label>("artist-name");
        this.albumName  = root.Q<Label>("album-name");

        this.trackDuration = root.Q<Label>("track-duration");
        this.trackPlays    = root.Q<Label>("track-plays");

        this.trackSource = root.Q<Label>("track-source");
        this.trackShard  = root.Q<Label>("track-shard");
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
        SetLabelText(this.albumName, track.album?.DisplayName(), "None");

        this.trackDuration.text = track.DisplayDuration();
        this.trackPlays.text    = track.totalPlays.ToString();

        SetLabelText(this.trackSource, track.audio_file, "Default");
        SetLabelText(this.trackShard,  track.shard,      "None Set");
    }


    void SetLabelText(Label label, string source, string default_text)
    {
        if (source is not null) {
            label.text = source;
            label.RemoveFromClassList("value-default");
            label.AddToClassList("value-set");
        }
        else {
            label.text = default_text;
            label.RemoveFromClassList("value-set");
            label.AddToClassList("value-default");
        }
    }
}
