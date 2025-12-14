using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class PlaybackButtonsScript : MonoBehaviour
{
    public VisualElement ui;

    public PauseButton pauseButton;
    public PrevButton prevButton;
    public NextButton nextButton;


    void Awake()
    {
        this.ui = GetComponent<UIDocument>().rootVisualElement;
    }

    void OnEnable()
    {
        this.pauseButton = new PauseButton(this.ui);
        this.prevButton = new PrevButton(this.ui);
        this.nextButton = new NextButton(this.ui);
    }

    void Start()
    {
        this.pauseButton.BindListeners();
        this.prevButton.BindListeners();
        this.nextButton.BindListeners();
    }
}


public class PauseButton : Bases.Clicky
{
    public PauseButton(VisualElement ui) : base(ui, "pause")
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


public class PrevButton : Bases.Clicky
{
    public PrevButton(VisualElement ui) : base(ui, "prev")
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


public class NextButton : Bases.Clicky
{
    public NextButton(VisualElement ui) : base(ui, "next")
    {
        Disable();
    }

    public override void BindListeners()
    {
        this.button.clicked += () => Exec.Audio.PlayNext();
        
        Exec.Audio.onQueueUpdated += () =>
            this.button.SetEnabled(Exec.Audio.queuedTracks.Count > 0);
    }
}
