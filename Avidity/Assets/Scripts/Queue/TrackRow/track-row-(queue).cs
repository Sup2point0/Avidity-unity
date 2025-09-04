using UnityEngine;
using TMPro;

using Avidity;


public class TrackRowQueueScript : ButtonScript
{
    public GameObject displayedTrackName;
    public GameObject displayedArtistName;

    public Track track;


    public void Init(Track track)
    {
        this.track = track;
    }

    protected override void Start()
    {
        base.Start();

        displayedTrackName.GetComponent<TMP_Text>().text = track.name ?? "<UNTITLED TRACK>";
        displayedArtistName.GetComponent<TMP_Text>().text = Artist.DisplayNames(track.artists) ?? "<NO ARTIST>";
    }

    protected override void OnClick()
    {
        Exec.Scene.SelectTrack(this.track);
    }
}
