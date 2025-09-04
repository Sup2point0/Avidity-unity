using UnityEngine;

using Avidity;


public class QueueTrackButtonScript : ButtonScript
{
    public Track track;


    protected override void Start()
    {
        base.Start();

        this.track = GetComponentInParent<TrackRowScript>().track;
    }

    protected override void OnClick()
    {
        Exec.Audio.AddToQueue(track);
    }
}
