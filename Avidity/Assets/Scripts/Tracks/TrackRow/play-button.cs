using UnityEngine;


public class PlayTrackButtonScript : ButtonScript
{
    public Track track;


    protected override void Start()
    {
        base.Start();

        this.track = GetComponentInParent<TrackRowScript>().track;
    }

    void Update()
    { 
    }

    protected override void OnClick()
    {
        Exec.Audio.PlayNow(track);
    }
}
