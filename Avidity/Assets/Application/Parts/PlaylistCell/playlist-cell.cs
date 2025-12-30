using System;

using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


[UxmlElement]
public partial class PlaylistCell : VisualElement, Bases.IBindableItem<Playlist>
{
    public Playlist playlist;

    public PlayClicky  playClicky;
    public QueueClicky queueClicky;
    
    public Label listName;
    public Label listTrackCount;

    private IManipulator click_manipulator;


    public PlaylistCell() {}

    public void InitFromUxml(VisualTreeAsset tree)
    {
        tree.CloneTree(this);

        this.playClicky = new PlayClicky(this);
        this.playClicky.BindListeners();

        this.queueClicky = new QueueClicky(this);
        this.queueClicky.BindListeners();

        this.listName       = this.Q<Label>("list-name");
        this.listTrackCount = this.Q<Label>("track-count");
    }

    public void Bind(Playlist playlist)
    {
        this.playlist = playlist;

        this.listName.text = playlist.DisplayName();
        this.listTrackCount.text = playlist.trackCount.ToString();

        this.click_manipulator = new Clickable(e => {
            Exec.Scene.SelectEntity(Bases.EntityType.Playlist, playlist);
        });
        this.AddManipulator(this.click_manipulator);
    }

    public void Unbind()
    {
        if (this.click_manipulator != null) this.RemoveManipulator(this.click_manipulator);
    }


#region BUTTONS

    public class PlayClicky : Bases.ClickyScript
    {
        public PlaylistCell root;


        public PlayClicky(PlaylistCell root) : base(root, "play")
        {
            this.root = root;
        }

        public override void BindListeners()
        {
            this.button.clicked += () => {
                Exec.Audio.PlayNow(root.playlist);
            };
        }
    }


    public class QueueClicky : Bases.ClickyScript
    {
        public PlaylistCell root;


        public QueueClicky(PlaylistCell root) : base(root, "queue")
        {
            this.root = root;
        }

        public override void BindListeners()
        {
            this.button.clicked += () => {
                Exec.Audio.QueueTracks(root.playlist.tracks);
            };
        }
    }

#endregion

}
