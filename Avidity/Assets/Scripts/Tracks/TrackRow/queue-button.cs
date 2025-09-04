using UnityEngine;

using Avidity;


public class QueueTrackButtonScript : Avidity.Bases.ButtonScript
{
    public Track track;


    protected override void Start()
    {
        base.Start();

        this.track = GetComponentInParent<TrackRowWindowScript>().track;
    }

    protected override void OnClick()
    {
        Exec.Audio.AddToQueue(track);
    }
}
