using UnityEngine;


public class PlayTrackButtonScript : ButtonScript
{
    public Track track;


    protected override void Start()
    {
        base.Start();

        track = new Track() {
            shard = "synthesis"
        };
    }

    void Update()
    { 
    }

    protected override void OnClick()
    {
        Exec.Audio.PlayNow(track);
    }
}
