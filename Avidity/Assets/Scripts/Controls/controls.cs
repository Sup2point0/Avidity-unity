using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlaybackControlsScript : Avidity.Bases.UIElementScript
{
    public TMP_Text displayedTrackName;
    public TMP_Text displayedArtistName;

    
    void Awake()
    {
        GetComponent<Image>().color = colours.Back.Protive;
    }
}
