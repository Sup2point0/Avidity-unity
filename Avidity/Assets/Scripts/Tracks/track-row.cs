using UnityEngine;
using UnityEngine.UI;


public class TrackRowScript : MonoBehaviour
{
    public Track track;


    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);

        track = new Track() {
            shard = "synthesis"
        };
    }

    void Update()
    {
        
    }

    void OnClick()
    {
        Exec.Audio.PlayTrack(track);
    }
}
