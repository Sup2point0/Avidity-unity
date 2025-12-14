using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class PlaybackButtonsScript : Bases.InterfaceController
{
    public PauseClicky pauseClicky;
    public PrevClicky prevClicky;
    public NextClicky nextClicky;


    void OnEnable()
    {
        this.pauseClicky = new PauseClicky(this.ui);
        this.prevClicky = new PrevClicky(this.ui);
        this.nextClicky = new NextClicky(this.ui);
    }

    void Start()
    {
        this.pauseClicky.BindListeners();
        this.prevClicky.BindListeners();
        this.nextClicky.BindListeners();
    }
}


public class PauseClicky : Bases.Clicky
{
    public PauseClicky(VisualElement ui) : base(ui, "pause")
    {
        Disable();
    }

    public override void BindListeners()
    {
        this.button.clicked += () => Exec.Audio.TogglePause();
        Exec.Audio.onTrackPlayed += Enable;
        Exec.Audio.onTrackCleared += Disable;
    }
}


public class PrevClicky : Bases.Clicky
{
    public PrevClicky(VisualElement ui) : base(ui, "prev")
    {
        Disable();
    }

    public override void BindListeners()
    {
        this.button.clicked += () => Exec.Audio.Restart();
        Exec.Audio.onTrackPlayed += Enable;
        Exec.Audio.onTrackCleared += Disable;
    }
}


public class NextClicky : Bases.Clicky
{
    public NextClicky(VisualElement ui) : base(ui, "next")
    {
        Disable();
    }

    public override void BindListeners()
    {
        this.button.clicked += () => Exec.Audio.PlayNext();

        Exec.Audio.onQueueUpdated += UpdateEnabled;
    }

    void UpdateEnabled()
    {
        this.button.SetEnabled(Exec.Audio.queuedTracks.Count > 0);
    }
}
