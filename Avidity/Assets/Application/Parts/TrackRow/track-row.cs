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

        this.playClicky = new PlayClicky(this);
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

        this.trackName.text = track.DisplayName();
        this.artistName.text = Artist.DisplayNames(track.artists);
        this.trackDuration.text = track.DisplayDuration();

        this.AddManipulator(new Clickable(e => {
            Exec.Scene.SelectTrack(track);
        }));
    }
}


public class PlayClicky : Bases.Clicky
{
    public TrackRow root;


    public PlayClicky(TrackRow root) : base(root, "play")
    {
        this.root = root;
    }

    public override void BindListeners()
    {
        if ((root.ctx & TrackRow.Context.QUEUE) != 0) {
            this.button.clicked += () => {
                Exec.Audio.PlayNow(root.track, root.qid);
                
                /* Should always be non-null if `ctx` is `QUEUE`, but better safe than sorry... */
                // if (this.qid.HasValue) {
                //     Exec.Audio.UnqueueTrack(this.qid.Value);
                // }
            };
        }
        else {
            this.button.clicked += () => Exec.Audio.PlayNow(root.track);
        }
    }
}


public class QueueClicky : Bases.Clicky
{
    public TrackRow root;


    public QueueClicky(TrackRow root) : base(root, "queue")
    {
        this.root = root;
    }

    public override void BindListeners()
    {
        this.button.clicked += () => Exec.Audio.QueueTrack(root.track);
    }
}


public class RemoveClicky : Bases.Clicky
{
    public TrackRow root;


    public RemoveClicky(TrackRow root) : base(root, "remove")
    {
        this.root = root;
    }

    public override void BindListeners()
    {
        this.button.clicked += () => {
            if (root.qid.HasValue) {
                Exec.Audio.UnqueueTrack(root.qid.Value);
            } else {
                Debug.Log("ERROR: No queue ID supplied?");
            }
        };
    }
}
