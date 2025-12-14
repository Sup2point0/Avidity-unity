using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class PlaybackButtonsScript : MonoBehaviour
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

    void Start()
    {
        this.pauseButton.BindListeners();
        this.prevButton.BindListeners();
        this.skipButton.BindListeners();
    }
}


public class PauseButton
{
    public Button button;

    public PauseButton(VisualElement ui)
    {
        this.button = ui.Q<Button>("pause");
        this.button.SetEnabled(false);
        
        Debug.Log("SETTING");
        this.button.clicked += () => {
            Debug.Log("CLICKED");
            Exec.Audio.TogglePause();
        };
    }

    public void BindListeners()
    {
        Exec.Audio.onTrackPlayed  += () => this.button.SetEnabled(true);
        Exec.Audio.onTrackCleared += () => this.button.SetEnabled(false);
    }
}


public class PrevButton
{
    public Button button;

    public PrevButton(VisualElement ui)
    {
        this.button = ui.Q<Button>("prev");
        this.button.SetEnabled(false);
        this.button.clicked += () => Exec.Audio.Restart();
    }

    public void BindListeners()
    {
        Exec.Audio.onTrackPlayed  += () => this.button.SetEnabled(true);
        Exec.Audio.onTrackCleared += () => this.button.SetEnabled(false);
    }
}


public class SkipButton
{
    public Button button;

    public SkipButton(VisualElement ui)
    {
        this.button = ui.Q<Button>("skip");
        this.button.SetEnabled(false);
        this.button.clicked += () => Exec.Audio.PlayNext();
    }

    public void BindListeners()
    {
        Exec.Audio.onQueueUpdated += () =>
            this.button.SetEnabled(Exec.Audio.queuedTracks.Count > 0);
    }
}
