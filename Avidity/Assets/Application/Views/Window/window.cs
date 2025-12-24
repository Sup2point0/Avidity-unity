using UnityEngine;
using UnityEngine.UIElements;

using System.Collections.Generic;

using Avidity;

using SceneExec = SceneExecutive;


public class WindowController : Bases.InterfaceController
{
    public Dictionary<SceneExec.Tab, VisualElement> tabWindows = new();


    void OnEnable()
    {
        var root = this.ui.Q<VisualElement>("window");

        this.tabWindows[SceneExec.Tab.Tracks] = root.Q<VisualElement>("tracks");
    }

    void Start()
    {
        foreach (var kvp in this.tabWindows)
        {
            var tab = kvp.Key;
            var window = kvp.Value;
            
            Exec.Scene.onTabChanged += () => {
                window.style.display =
                    Exec.Scene.currentTab == tab
                    ? DisplayStyle.Flex
                    : DisplayStyle.None
                ;
            };
        }
    }
}
