using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Avidity;


public class TrackRowQueueContainerScript : MonoBehaviour
{
    public GameObject contentContainer;
    public GameObject trackRowPrefab;

    public List<Track> displayedTracks;


    void Start()
    {
        Exec.Audio.onQueueUpdated += OnQueueUpdated;
        RegenerateTrackRows();
    }

    
    void OnQueueUpdated()
    {
        displayedTracks = Exec.Audio.queuedTracks.ToList();
        RegenerateTrackRows();
    }


    private void RegenerateTrackRows()
    {
        GameObject track_row;

        foreach (var track in this.displayedTracks) {
            track_row = Instantiate(trackRowPrefab, contentContainer.transform);
            Debug.Log(track_row.GetComponent<TrackRowQueueScript>());
            track_row.GetComponent<TrackRowQueueScript>().Init(track);
        }
    }
}
