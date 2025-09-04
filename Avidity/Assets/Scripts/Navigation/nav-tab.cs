using UnityEngine;
using UnityEngine.UI;

using Avidity;


public class NavigationTabScript : Avidity.Bases.ButtonScript
{
    public SceneExecutive.Tab tab;


    protected override void OnClick()
    {
        Exec.Scene.NavigateToTab(this.tab);
    }
}
