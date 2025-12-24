using UnityEngine;

using Avidity;


public class PushTrackButtonQueueScript : Avidity.Bases.ButtonScript
{
    public Track track;


    protected override void Start()
    {
        base.Start();

        this.track = GetComponentInParent<TrackRowQueueScript>().track;
    }

    protected override void OnClick()
    {
        Exec.Audio.PlayNow(track);
    }
}
