using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


[UxmlElement]
public partial class TrackRow : VisualElement
{
    public PlayClicky playClicky;
    public QueueClicky queueClicky;
    
    public Label trackName;
    public Label artistName;
    public Label trackDuration;

    public Track track;


    public TrackRow() {}

    public TrackRow(VisualTreeAsset tree)
    {
        tree.CloneTree(this);

        this.playClicky = new PlayClicky(this);
        this.playClicky.BindListeners();
        this.queueClicky = new QueueClicky(this);
        this.queueClicky.BindListeners();

        this.trackName     = this.Q<Label>("track-name");
        this.artistName    = this.Q<Label>("artist-name");
        this.trackDuration = this.Q<Label>("track-duration");
    }


    public void Bind(Track track)
    {
        this.track = track;
        this.playClicky.track = track;
        this.queueClicky.track = track;

        this.trackName.text = track.name;
        this.artistName.text = Artist.DisplayNames(track.artists);
        this.trackDuration.text = track.DisplayDuration();

        this.AddManipulator(new Clickable(e =>
        {
            Exec.Scene.SelectTrack(track);
        }));
    }
}


public class PlayClicky : Bases.Clicky
{
    public Track track;


    public PlayClicky(VisualElement ui) : base(ui, "play") {}

    public override void BindListeners()
    {
        this.button.clicked += () => Exec.Audio.PlayNow(this.track);
    }
}


public class QueueClicky : Bases.Clicky
{
    public Track track;


    public QueueClicky(VisualElement ui) : base(ui, "queue") {}

    public override void BindListeners()
    {
        this.button.clicked += () => Exec.Audio.AddToQueue(this.track);
    }
}
