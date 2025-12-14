using UnityEngine;
using UnityEngine.UIElements;

using System.Collections.Generic;

using Avidity;


using SceneExec = SceneExecutive;


public class NavigationBarScript : Bases.InterfaceController
{
    public Dictionary<SceneExec.Tab, Button> tabButtons = new();


    void OnEnable()
    {
        var root = this.ui.Q<VisualElement>("navigation");
        this.tabButtons[SceneExec.Tab.Playlists] = root.Q<Button>("playlists");
        this.tabButtons[SceneExec.Tab.Tracks]    = root.Q<Button>("tracks");
        this.tabButtons[SceneExec.Tab.Artists]   = root.Q<Button>("artists");
    }

    void Start()
    {
        foreach (var kvp in this.tabButtons)
        {
            var tab = kvp.Key;
            var button = kvp.Value;
            Debug.Log($"tab = {tab}");
            button.clicked += () => Exec.Scene.currentTab = tab;
        }
    }
}
