#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

using Avidity;
using Avid = Avidity;
using Unity.VisualScripting;


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
    public Action? onSelectionChanged;

#endregion


#region FIELDS

    [Header("Configuration")]

    public int frameRateWhenActive = 0;
    public int frameRateWhenIdle = 0;
    public int frameRateWhenUnfocused = 0;
    public float secondsBeforeIdle;


    [Header("Connections")]

    public PlayerInput? playerInput;


    [Header("State")]

    public Tab currentTab { get; private set; } = Tab.Tracks;

    /// <summary> The type of the currently selected entity. </summary>
    public Bases.EntityType selectedEntityType { get; private set; }
        = Bases.EntityType.NoSelection;

    /// <summary> The currently selected entity to be shown in the Selector. </summary>
    /// <remarks>
    /// Use `.selectedEntityType` to determine its type and downcast as appropriate.
    /// </remarks>
    public object? selectedEntity { get; private set; }

    
#if UNITY_EDITOR
    [Header("Debug")]

    [SerializeField] private int             numberOfItemsInPreviews;
    [SerializeField] private int             totalTrackCount;
    [SerializeField] private List<Track>?    applicationTracks;
    [SerializeField] private List<Playlist>? applicationPlaylists;
    [SerializeField] private List<Artist>?   applicationArtists;

#endif

    [SerializeField] private bool is_focused;
    [SerializeField] private bool is_idle;
    [SerializeField] private float until_idle;


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

#if UNITY_EDITOR
        this.totalTrackCount      = Persistence.data.tracks.Count;
        this.applicationTracks    = Persistence.data.tracks.Values.Take(this.numberOfItemsInPreviews).ToList();
        this.applicationArtists   = Persistence.data.artists.Values.Take(this.numberOfItemsInPreviews).ToList();
        this.applicationPlaylists = Persistence.data.playlists.Values.Take(this.numberOfItemsInPreviews).ToList();
#endif
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

    public void SelectEntity<Entity>(
        Bases.EntityType type,
        Entity entity
    )
        where Entity : Bases.ISelectableEntity
    {
        this.selectedEntityType = type;
        this.selectedEntity = entity;

        onSelectionChanged?.Invoke();

        if (entity is Track) {
            /* NOTE: Only load if necessary! */
            if ((entity as Track)!.duration is null) {
                async void UpdateInfo() {
                    await Exec.Audio.LoadClipAsync(entity as Track);
                    onSelectionChanged?.Invoke();
                }

                UpdateInfo();
            }
        }
    }
    
#endregion
}
