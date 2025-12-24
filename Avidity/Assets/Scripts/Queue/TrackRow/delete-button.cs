using UnityEngine;

using Avidity;


public class DeleteTrackButtonQueueScript : Avidity.Bases.ButtonScript
{
    public Track track;


    protected override void Start()
    {
        base.Start();

        this.track = GetComponentInParent<TrackRowQueueScript>().track;
    }

    protected override void OnClick()
    {
    }
}
