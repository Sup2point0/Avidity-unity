using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

using Avidity;
using Avid = Avidity;


/// <summary> The scene manager. </summary>
public class SceneExecutive : MonoBehaviour
{

#region ENUMS

    /// <summary> A window tab that can be navigated to in the application. </summary>
    public enum Tab {
        Config, Files, Home, Tracks, Playlists, Artists,
    }

#endregion


#region DELEGATES

    /// <summary> Fired when the window tab is changed. </summary>
    public Action onTabChanged;

    /// <summary> Fired when a track is selected. </summary>
    public Action onTrackSelected;

#endregion


    [Header("Configuration")]

    public int frameRateWhenActive = 0;
    public int frameRateWhenIdle = 0;
    public int frameRateWhenUnfocused = 0;
    public float secondsBeforeIdle;


    [Header("Connections")]

    public PlayerInput playerInput;
    public Canvas mainCanvas;


    [Header("State")]

    [SerializeField] public Tab currentTab { get; private set; } = Tab.Tracks;

    /// <summary> The currently selected track to be shown in the Selector. Note this can be `null`, but we're not marking it `Track?` just to avoid `.Value` shenanigans. TODO: Might change in future?
    /// </summary>
    public Track selectedTrack { get; private set; }

    public Bases.SelectableObjectType selectedObjectType { get; private set; }


#region PRIVATE

    public bool is_focused;
    public bool is_idle;
    public float until_idle;

#endregion


#region UNITY

    void Awake()
    {
        this.currentTab = Persistence.options?.default_tab ?? Tab.Home; 
        Exec.Scene = this;

        this.ToActive();
    }

    void Start()
    {
        this.onTabChanged?.Invoke();
    }

    void Update()
    {
        // Debug.Log($"Application.targetFrameRate = {Application.targetFrameRate}");
        if (!is_focused) return;

        if (this.until_idle < 0) {
            this.ToIdle();
        }
        else if (!this.is_idle) {
            this.until_idle -= Time.deltaTime;
        }
    }

    void OnApplicationFocus(bool isFocused)
    {
        if (isFocused) {
            this.ToFocused();
        }
        else {
            this.ToUnfocused();
        }
    }

    void OnPoint()
    {
        if (!is_focused) return;
        
        if (this.is_idle) {
            this.ToActive();
        }
        else {
            this.until_idle = this.secondsBeforeIdle;
        }
    }

#endregion


#region INTERNAL

    private void ToActive()
    {
        this.is_idle = false;
        this.until_idle = this.secondsBeforeIdle;

        // QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = this.frameRateWhenActive;
    }

    private void ToIdle()
    {
        this.is_idle = true;
        this.until_idle = 0;

        // QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = this.frameRateWhenIdle;
    }

    private void ToFocused()
    {
        this.is_focused = true;

        // QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = this.frameRateWhenActive;
        // this.playerInput.enabled = true;
        this.mainCanvas.enabled = true;
    }

    private void ToUnfocused()
    {
        this.is_focused = false;

        // QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = this.frameRateWhenUnfocused;
        this.playerInput.enabled = false;
        this.mainCanvas.enabled = false;
    }
    
#endregion


#region INTERFACE
    
    public void NavigateToTab(Tab tab)
    {
        this.currentTab = tab;
        onTabChanged?.Invoke();
    }

    public void SelectTrack(Track track)
    {
        this.selectedTrack = track;
        this.selectedObjectType = Bases.SelectableObjectType.Track;
        
        onTrackSelected?.Invoke();

        async void UpdateInfo() {
            await Exec.Audio.LoadClipAsync(track);
            onTrackSelected?.Invoke();
        }

        /* NOTE: Only load if necessary! */
        if (track.duration is null) {
            UpdateInfo();
        }
    }
    
#endregion
}
