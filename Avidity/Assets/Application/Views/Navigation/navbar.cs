using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

using System.Collections.Generic;

using Avidity;

using SceneExec = SceneExecutive;


public class NavigationBarController : Bases.InterfaceController
{
    public Dictionary<SceneExec.Tab, Button> tabButtons = new();

    public Button quitButton;


    void OnEnable()
    {
        var root = this.ui.Q<VisualElement>("navigation");

        this.tabButtons[SceneExec.Tab.Config]   = root.Q<Button>("config");
        this.tabButtons[SceneExec.Tab.Files]     = root.Q<Button>("files");
        this.tabButtons[SceneExec.Tab.Home]      = root.Q<Button>("home");
        this.tabButtons[SceneExec.Tab.Tracks]    = root.Q<Button>("tracks");
        this.tabButtons[SceneExec.Tab.Playlists] = root.Q<Button>("playlists");
        this.tabButtons[SceneExec.Tab.Artists]   = root.Q<Button>("artists");

        this.quitButton = root.Q<Button>("quit");
    }

    void Start()
    {
        foreach (var kvp in this.tabButtons)
        {
            var tab = kvp.Key;
            var button = kvp.Value;
            button.clicked += () => Exec.Scene.NavigateToTab(tab);
        }

        this.quitButton.clicked += () => Application.Quit();
    }
}
