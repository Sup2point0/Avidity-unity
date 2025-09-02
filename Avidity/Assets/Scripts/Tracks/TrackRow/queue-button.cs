using UnityEngine;


public class QueueTrackButtonScript : ButtonScript
{
    public Track track;


    protected override void Start()
    {
        base.Start();

        track = new Track() {
            shard = "colour-mourning"
        };
    }

    void Update()
    { 
    }

    protected override void OnClick()
    {
        Exec.Audio.AddToQueue(track);
    }
}
