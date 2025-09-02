using UnityEngine;
using UnityEngine.UI;


public class TrackRowScript : MonoBehaviour
{
    public Button button;

    public Track track;


    void Start()
    {
        button.onClick.AddListener(OnClick);

        track = new Track() {
            shard = "synthesis"
        };
    }

    void Update()
    { 
    }

    void OnClick()
    {
        Exec.Audio.Play(track);
    }
}
