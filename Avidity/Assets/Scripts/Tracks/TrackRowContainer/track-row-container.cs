using System.Collections.Generic;
using System.Linq;

using UnityEngine;


public class TrackRowContainerScript : MonoBehaviour
{
    public List<Track> displayedTracks;

    public GameObject contentContainer;
    public GameObject trackRowPrefab;


    void Awake()
    {
        displayedTracks = Data.Tracks.ToList();
    }

    void Start()
    {
        GameObject track_row;

        foreach (var track in this.displayedTracks) {
            track_row = Instantiate(trackRowPrefab, contentContainer.transform);
            track_row.GetComponent<TrackRowScript>().Init(track);
        }
    }

    void Update()
    {
    }
}
