using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class QueuePaneController : Bases.InterfaceController
{
    public ListView listView;
    public VisualTreeAsset trackRow;


    void OnEnable()
    {
        this.listView = this.ui.Q<ListView>("queue-list");
        this.listView.makeItem = MakeItem;
        this.listView.bindItem = BindItem;
    }

    void Start()
    {
        this.listView.itemsSource = Exec.Audio.queuedTracks;
        Exec.Audio.onQueueUpdated += () => this.listView.RefreshItems();
    }


    VisualElement MakeItem()
    {
        return new TrackRow(this.trackRow);
    }

    void BindItem(VisualElement elem, int idx)
    {
        (elem as TrackRow).Bind(Exec.Audio.queuedTracks[idx]);
    }
}
