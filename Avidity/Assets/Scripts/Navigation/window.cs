using UnityEngine;

using Avidity;


/// <summary> A window parent which disables its children <c>GameObject</c>s when the current navigation tab is different to the window's tab. </summary>
public class WindowScript : MonoBehaviour
{
    public SceneExecutive.Tab tab;


    void Start()
    {
        Exec.Scene.onTabNavigate = OnTabNavigate;
    }

    
    void OnTabNavigate()
    {
        gameObject.SetActive(Exec.Scene.currentTab == tab);
    }
}
