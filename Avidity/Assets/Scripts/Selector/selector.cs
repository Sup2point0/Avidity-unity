using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SelectorScript : MonoBehaviour
{
    public TMP_Text trackNameText;
    public TMP_Text artistNameText;
    public TMP_Text albumNameText;


    void Start()
    {
        Exec.Scene.onTrackSelect += OnTrackSelect;
    }

    void Update()
    {}


    void OnTrackSelect()
    {
        trackNameText.text = Exec.Scene.selectedTrack.name;
        artistNameText.text = Artist.DisplayNames(Exec.Scene.selectedTrack.artists);
        albumNameText.text = Exec.Scene.selectedTrack.album?.name ?? "";
    }
}
