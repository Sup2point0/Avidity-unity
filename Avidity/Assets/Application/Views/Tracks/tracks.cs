using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class TracksWindowController : Bases.InterfaceController
{
    public ListView listView;
    public VisualTreeAsset trackRow;

    public List<Track> displayedTracks = new(); 


    void OnEnable()
    {
        RegenerateTracks();

        this.listView = this.ui.Q<ListView>("tracks-list");
        this.listView.itemsSource = this.displayedTracks;
        this.listView.makeItem = MakeItem;
        this.listView.bindItem = BindItem;
    }


    void RegenerateTracks()
    {
        this.displayedTracks = Persistence.data.tracks.Values.ToList();
    }

    VisualElement MakeItem()
    {
        return new TrackRow(this.trackRow);
    }

    void BindItem(VisualElement elem, int idx)
    {
        (elem as TrackRow).Bind(this.displayedTracks[idx]);
    }
}
