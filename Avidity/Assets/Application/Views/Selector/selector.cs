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
    public Label trackAudio;
    public Label trackCover;


    void OnEnable()
    {
        var root = this.ui.Q<VisualElement>("selector");

        this.trackName  = root.Q<Label>("track-name");
        this.artistName = root.Q<Label>("artist-name");
        this.albumName  = root.Q<Label>("album-name");

        this.trackDuration = root.Q<Label>("track-duration");
        this.trackPlays    = root.Q<Label>("track-plays");

        this.trackShard = root.Q<Label>("track-shard");
        this.trackAudio = root.Q<Label>("track-audio");
        this.trackCover = root.Q<Label>("track-cover");
    }

    void Start()
    {
        Exec.Scene.onSelectionChanged += OnSelectionChanged;
    }


    void OnSelectionChanged()
    {
        var track = Exec.Scene.selectedEntity as Track;

        this.trackName.text  = track.DisplayName();
        this.artistName.text = Artist.DisplayNames(track.artists);
        SetLabelText(this.albumName, track.album?.DisplayName(), "None");

        this.trackDuration.text = track.DisplayDuration();
        this.trackPlays.text    = track.totalPlays.ToString();

        SetLabelText(this.trackShard, track.weak_shard, "None Set");
        SetLabelText(this.trackAudio, track.audio_file, "Default");
        SetLabelText(this.trackCover, track.cover_file, "Default");
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
