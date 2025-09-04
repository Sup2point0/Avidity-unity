using UnityEngine;

using Avidity;


public class SkipButtonScript : Avidity.Bases.ButtonScript
{
    void Awake()
    {
        button.interactable = false;
    }
    
    protected override void Start()
    {
        base.Start();

        Exec.Audio.onQueueUpdated += OnQueueUpdated;
    }


    protected override void OnClick()
    {
        Exec.Audio.PlayNext();
    }


    void OnQueueUpdated()
    {
        button.interactable = (Exec.Audio.queuedTracks.Count > 0);
    }
}
