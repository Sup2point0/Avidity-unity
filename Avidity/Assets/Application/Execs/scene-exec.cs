#nullable enable

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

    public Action? onTabChanged;
    public Action? onTrackSelected;
    public Action? onSelectionChanged;

#endregion


    [Header("Configuration")]

    public int frameRateWhenActive = 0;
    public int frameRateWhenIdle = 0;
    public int frameRateWhenUnfocused = 0;
    public float secondsBeforeIdle;


    [Header("Connections")]

    public PlayerInput? playerInput;


    [Header("State")]

    [SerializeField] public Tab currentTab { get; private set; } = Tab.Tracks;

    /// <summary> The currently selected track to be shown in the Selector. Note this can be `null`, but we're not marking it `Track?` just to avoid `.Value` shenanigans. TODO: Might change in future?
    /// </summary>
    public Track? selectedTrack { get; private set; }

// TODO: migrating
    /// <summary> The type of the currently selected entity. </summary>
    public Bases.SelectableEntityType selectedEntityType { get; private set; }
        = Bases.SelectableEntityType.NoSelection;

    /// <summary> The currently selected entity. Use `.selectedEntityType` to determine its type and downcast as appropriate. </summary>
    public object? selectedEntity { get; private set; }


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
    }

    private void ToUnfocused()
    {
        this.is_focused = false;

        // QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = this.frameRateWhenUnfocused;
        this.playerInput!.enabled = false;
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
        
        onTrackSelected?.Invoke();

    // TODO: part of migration
        this.selectedEntityType = Bases.SelectableEntityType.Track;
        this.selectedEntity = track;

        onSelectionChanged?.Invoke();

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
