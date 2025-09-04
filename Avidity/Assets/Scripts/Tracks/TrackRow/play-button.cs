using UnityEngine;

using Avidity;


public class PlayTrackButtonScript : Avidity.Bases.ButtonScript
{
    public Track track;


    protected override void Start()
    {
        base.Start();

        this.track = GetComponentInParent<TrackRowWindowScript>().track;
    }


    protected override void OnClick()
    {
        Exec.Audio.PlayNow(track);
    }
}
