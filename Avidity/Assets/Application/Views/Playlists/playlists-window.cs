using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

using Avidity;

using Row = GridRow<PlaylistCell, Avidity.Playlist>;


public class PlaylistsWindowController : Bases.InterfaceController
{
    public const int CELLS_PER_ROW = 3;

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
        this.displayedPlaylists = Persistence.data.playlists.Values.Chunked(CELLS_PER_ROW).ToList();
    }

    Row MakeItem()
        => new("playlist-row", CELLS_PER_ROW, this.cellUxml);

    void BindItem(VisualElement elem, int idx)
    {
        (elem as Row).BindItems(this.displayedPlaylists[idx]);
    }

    void UnbindItem(VisualElement elem, int idx)
    {
        (elem as Row).UnbindItems();
    }
}
