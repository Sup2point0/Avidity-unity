using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Avidity;


public class PlaybackControlsScript : Avidity.Bases.UIElementScript
{
    public TMP_Text displayedTrackName;
    public TMP_Text displayedArtistName;


    void Awake()
    {
        GetComponent<Image>().color = colours.Back.Protive;
    }

    void Start()
    {
        Exec.Audio.onTrackPlayed += OnTrackPlayed;
        Exec.Audio.onTrackCleared += OnTrackCleared;
    }


    void OnTrackPlayed()
    {
        displayedTrackName.text = Exec.Audio.activeTrack.name;
        displayedArtistName.text = Artist.DisplayNames(Exec.Audio.activeTrack.artists);
    }

    void OnTrackCleared()
    {
        displayedTrackName.text = "No Track Selected";
        displayedArtistName.text = "";
    }
}
