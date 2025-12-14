using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class PlaybackControls_Script : MonoBehaviour
{
    public VisualElement ui;

    public PauseButton pauseButton;
    public PrevButton prevButton;
    public SkipButton skipButton;


    void Awake()
    {
        this.ui = GetComponent<UIDocument>().rootVisualElement;
    }

    void OnEnable()
    {
        this.pauseButton = new PauseButton(this.ui);
        this.prevButton = new PrevButton(this.ui);
        this.skipButton = new SkipButton(this.ui);
    }
}


public class PauseButton
{
    public Button button;

    public PauseButton(VisualElement ui)
    {
        this.button = ui.Q<Button>("pause");
        this.button.SetEnabled(false);
        this.button.clicked += Exec.Audio.TogglePause;

        Exec.Audio.onTrackPlayed += this.OnTrackPlayed;
        Exec.Audio.onTrackCleared += this.OnTrackCleared;
    }

    void OnTrackPlayed()
    {
        this.button.SetEnabled(true);
    }

    void OnTrackCleared()
    {
        this.button.SetEnabled(false);
    }
}


public class PrevButton
{
    public Button button;

    public PrevButton(VisualElement ui)
    {
        this.button = ui.Q<Button>("prev");
        this.button.SetEnabled(false);
        this.button.clicked += Exec.Audio.Restart;

        Exec.Audio.onTrackPlayed += this.OnTrackPlayed;
        Exec.Audio.onTrackCleared += () => {
            this.button.SetEnabled(false);
        };
    }

    void OnTrackPlayed()
    {
        this.button.SetEnabled(true);
    }

    void OnTrackCleared()
    {
        this.button.SetEnabled(false);
    }
}


public class SkipButton
{
    public Button button;

    public SkipButton(VisualElement ui)
    {
        this.button = ui.Q<Button>("skip");
        this.button.SetEnabled(false);
        this.button.clicked += Exec.Audio.PlayNext;

        Exec.Audio.onQueueUpdated += () => {
            this.button.SetEnabled(Exec.Audio.queuedTracks.Count > 0);
        };
    }  
}
