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


public class PauseButton
{
    public Button button;

    public PauseButton(VisualElement ui)
    {
        this.button = ui.Q<Button>("pause");
        this.button.SetEnabled(false);
    }

    public void BindListeners()
    {
        Debug.Log($"Exec.Audio = {Exec.Audio}");
        this.button.clicked += () => Exec.Audio.TogglePause();

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


public class NextButton
{
    public Button button;

    public NextButton(VisualElement ui)
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
