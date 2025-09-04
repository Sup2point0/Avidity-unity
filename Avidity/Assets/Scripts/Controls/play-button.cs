using UnityEngine;
using UnityEngine.UI;

using Avidity;


public class PlayButtonScript : Avidity.Bases.ButtonScript
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
        Exec.Audio.TogglePause();
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
