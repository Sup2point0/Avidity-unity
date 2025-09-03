using UnityEngine;
using TMPro;


public class TrackRowScript : MonoBehaviour
{
    public GameObject displayedName;

    public Track track;


    public void Init(Track track)
    {
        this.track = track;
    }

    void Start()
    {
        displayedName.GetComponent<TMP_Text>().text = track.name ?? "<ERROR>";
    }

    void Update()
    {
    }
}
