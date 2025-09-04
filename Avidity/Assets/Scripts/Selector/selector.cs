using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Avidity;


public class SelectorScript : Avidity.UIElementScript
{
    public TMP_Text trackNameText;
    public TMP_Text artistNameText;
    public TMP_Text albumNameText;


    void Awake()
    {
        GetComponent<Image>().color = colours.Back.Deutive;
    }

    void Start()
    {
        Exec.Scene.onTrackSelect += OnTrackSelect;
    }


    void OnTrackSelect()
    {
        trackNameText.text = Exec.Scene.selectedTrack.name;
        artistNameText.text = Artist.DisplayNames(Exec.Scene.selectedTrack.artists);
        albumNameText.text = Exec.Scene.selectedTrack.album?.name ?? "";
    }
}
