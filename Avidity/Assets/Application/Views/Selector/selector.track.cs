using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class TrackSelectorController : SelectorController
{
    public VisualElement root;

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
        this.root = this.ui.Q<VisualElement>("selector-track");

        this.trackName  = this.root.Q<Label>("track-name");
        this.artistName = this.root.Q<Label>("artist-name");
        this.albumName  = this.root.Q<Label>("album-name");

        this.trackDuration = this.root.Q<Label>("track-duration");
        this.trackPlays    = this.root.Q<Label>("track-plays");

        this.trackShard = this.root.Q<Label>("track-shard");
        this.trackAudio = this.root.Q<Label>("track-audio");
        this.trackCover = this.root.Q<Label>("track-cover");
    }

    protected override void Start()
    {
        Exec.Scene.onSelectionChanged += OnSelectionChanged;
    }


    void OnSelectionChanged()
    {
        if (Exec.Scene.selectedEntityType != Bases.EntityType.Track) {
            this.root.style.display = DisplayStyle.None;
            return;
        }

        this.root.style.display = DisplayStyle.Flex;
        var track = Exec.Scene.selectedEntity as Track;
        RegenerateDetails(track);
    }

    void RegenerateDetails(Track track)
    {
        this.trackName.text  = track.DisplayName();
        this.artistName.text = Artist.DisplayNames(track.artists);
        SetLabelText(this.albumName, track.album?.DisplayName(), "None");

        this.trackDuration.text = track.DisplayDuration();
        this.trackPlays.text    = track.totalPlays.ToString();

        this.trackShard.text = track.weak_shard;
        SetLabelText(this.trackAudio, track.audio_file, "Auto");
        SetLabelText(this.trackCover, track.cover_file, "Auto");
    }
}
