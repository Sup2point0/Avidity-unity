using System;

using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


[UxmlElement]
public partial class PlaylistCell : VisualElement, Bases.IBindableItem<Playlist>
{
    public Playlist playlist;

    public PlayClicky playClicky;
    
    public Label listName;
    public Label listTrackCount;


    public PlaylistCell() {}

    public void InitFromUxml(VisualTreeAsset tree)
    {
        tree.CloneTree(this);

        this.playClicky = new PlayClicky(this);
        this.playClicky.BindListeners();

        this.listName       = this.Q<Label>("list-name");
        this.listTrackCount = this.Q<Label>("track-count");
    }

    public void Bind(Playlist playlist)
    {
        this.playlist = playlist;

        this.listName.text = playlist.DisplayName();
        this.listTrackCount.text = playlist.trackCount.ToString();

        // TODO: Open playlist
    }

    public void Unbind() {}


#region 

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

#endregion

}
