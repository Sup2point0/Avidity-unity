using UnityEngine;

using Avidity;


public class RewindButtonScript : Avidity.Bases.ButtonScript
{
    void Awake()
    {
        button.interactable = false;
    }
    
    protected override void Start()
    {
        base.Start();

        Exec.Audio.onTrackPlayed += OnTrackPlayed;
        Exec.Audio.onTrackCleared += OnTrackCleared;
    }

    
    protected override void OnClick()
    {
        Exec.Audio.Restart();
    }

    
    void OnTrackPlayed()
    {
        button.interactable = true;
    }
    
    void OnTrackCleared()
    {
        button.interactable = false;
    }
}
