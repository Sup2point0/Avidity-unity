using System;

using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


[UxmlElement]
public partial class TrackRow : VisualElement
{
    [Flags]
    public enum Context {
        TRACKS   = 1,
        QUEUE    = 1 << 1,
        PLAYLIST = 1 << 2,
        ARTIST   = 1 << 3,
    }

    public Context ctx;
    public uint?   qid;
    public Track   track;

    public PlayClicky playClicky;
    public QueueClicky queueClicky;
    public RemoveClicky removeClicky;
    
    public Label trackName;
    public Label artistName;
    public Label trackDuration;


    public TrackRow() {}

    public TrackRow(VisualTreeAsset tree): this(tree, Context.TRACKS) {}

    public TrackRow(VisualTreeAsset tree, Context ctx)
    {
        tree.CloneTree(this);

        this.ctx = ctx;

        this.playClicky = new PlayClicky(this, ctx);
        this.playClicky.BindListeners();

        this.queueClicky = new QueueClicky(this);
        if (this.ctx != Context.QUEUE) {
            this.queueClicky.BindListeners();
        } else {
            this.queueClicky.button.style.display = DisplayStyle.None;
        }

        this.removeClicky = new RemoveClicky(this);
        if ((this.ctx & (Context.QUEUE | Context.PLAYLIST | Context.ARTIST)) != 0) {
            this.removeClicky.BindListeners();
        } else {
            this.removeClicky.button.style.display = DisplayStyle.None;
        }

        this.trackName     = this.Q<Label>("track-name");
        this.artistName    = this.Q<Label>("artist-name");
        this.trackDuration = this.Q<Label>("track-duration");
    }


    public void Bind(Track track, uint? qid = null)
    {
        this.qid   = qid;
        this.track = track;

        this.playClicky.track  = track;
        this.queueClicky.track = track;
        this.removeClicky.qid  = qid;

        this.trackName.text = track.name;
        this.artistName.text = Artist.DisplayNames(track.artists);
        this.trackDuration.text = track.DisplayDuration();

        this.AddManipulator(new Clickable(e => {
            Exec.Scene.SelectTrack(track);
        }));
    }
}


public class PlayClicky : Bases.Clicky
{
    public uint? qid;
    public TrackRow.Context ctx;
    public Track track;


    public PlayClicky(VisualElement ui, TrackRow.Context ctx, uint? qid = null) : base(ui, "play")
    {
        this.qid = qid;
        this.ctx = ctx;
    }

    public override void BindListeners()
    {
        if ((this.ctx & TrackRow.Context.QUEUE) != 0) {
            this.button.clicked += () => {
                Exec.Audio.PlayNow(this.track);
                
                /* Should always be non-null if `ctx` is `QUEUE`, but better safe than sorry... */
                if (this.qid.HasValue) {
                    Exec.Audio.RemoveFromQueue(this.qid.Value);
                }
            };
        }
        else {
            this.button.clicked += () => Exec.Audio.PlayNow(this.track);
        }
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


public class RemoveClicky : Bases.Clicky
{
    public uint? qid;


    public RemoveClicky(VisualElement ui) : base(ui, "remove") {}

    public override void BindListeners()
    {
        
        this.button.clicked += () => {
            if (this.qid.HasValue) {
                Exec.Audio.RemoveFromQueue(this.qid.Value);
            } else {
                Debug.Log("ERROR: No queue ID supplied?");
            }
        };
    }
}
