using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

using Avidity;

using Row = GridRow<PlaylistCell, Avidity.Playlist>;


public class PlaylistsWindowController : Bases.InterfaceController
{
    public ListView gridView;
    public VisualTreeAsset cellUxml;

    public List<Playlist[]> displayedPlaylists = new();


    void OnEnable()
    {
        RegeneratePlaylists();

        this.gridView = this.ui.Q<ListView>("playlists-grid");
        this.gridView.itemsSource = this.displayedPlaylists;
        this.gridView.makeItem    = MakeItem;
        this.gridView.bindItem    = BindItem;
        this.gridView.unbindItem  = UnbindItem;
    }


    void RegeneratePlaylists()
    {
        this.displayedPlaylists = (
            from list in Persistence.data.playlists.Values
            select new Playlist[] { list, list, list }
        ).ToList();
    }

    Row MakeItem()
        => new(3, this.cellUxml);

    void BindItem(VisualElement elem, int idx)
    {
        (elem as Row).BindItems(this.displayedPlaylists[idx]);
    }

    void UnbindItem(VisualElement elem, int idx)
    {
        (elem as Row).UnbindItems();
    }
}
