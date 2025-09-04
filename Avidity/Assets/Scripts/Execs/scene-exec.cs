using System;

using UnityEngine;

using Avidity;


/// <summary> The scene manager. </summary>
public class SceneExecutive : MonoBehaviour
{
    #region ENUMS

    public enum Tab {
        Tracks, Playlists, Artists
    }

    #endregion


    #region DELEGATES

    /// <summary> Fired when the window tab is changed. </summary>
    public Action onTabNavigate;

    /// <summary> Fired when a track is selected. </summary>
    public Action onTrackSelect;

    #endregion

    
    public Tab currentTab = Tab.Tracks;

    [SerializeField]
    public Track selectedTrack;


    #region UNITY

    void Awake()
    {
        Exec.Scene = this;
    }

    #endregion


    public void NavigateToTab(Tab tab)
    {
        this.currentTab = tab;
        onTabNavigate?.Invoke();
    }

    public void SelectTrack(Track track)
    {
        this.selectedTrack = track;
        onTrackSelect?.Invoke();
    }
}
