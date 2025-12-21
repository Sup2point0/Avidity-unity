using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class TracksWindowController : Bases.InterfaceController
{
    public ListView tracksList;
    public VisualTreeAsset trackRow;

    public List<Track> displayedTracks = new(); 


    void OnEnable()
    {
        this.tracksList = this.ui.Q<ListView>("tracks-list");
        this.tracksList.itemsSource = this.displayedTracks;
        this.tracksList.makeItem = MakeItem;
        this.tracksList.bindItem = BindItem;
        this.tracksList.selectionType = SelectionType.Single;
        
        RegenerateTracks();
    }


    void RegenerateTracks()
    {
        this.displayedTracks = Data.Tracks.ToList();
    }

    VisualElement MakeItem()
    {
        return new TrackRow(this.trackRow);
    }

    void BindItem(VisualElement elem, int idx)
    {
        (elem as TrackRow).track = this.displayedTracks[idx];
    }
}
