using System.Collections.Generic;
using System.Linq;

using UnityEngine;


public class TrackRowContainerWindowScript : MonoBehaviour
{
    public GameObject contentContainer;
    public GameObject trackRowPrefab;
    
    public List<Track> displayedTracks;


    void Awake()
    {
        displayedTracks = Data.Tracks.ToList();
    }

    void Start()
    {
        GameObject track_row;

        foreach (var track in this.displayedTracks) {
            track_row = Instantiate(trackRowPrefab, contentContainer.transform);
            track_row.GetComponent<TrackRowWindowScript>().Init(track);
        }
    }
}
