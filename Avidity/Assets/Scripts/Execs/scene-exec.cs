using UnityEngine;


/// <summary> The scene manager. </summary>
public class SceneExec : MonoBehaviour
{
    #region ENUMS

    public enum Tab {
        Tracks, Playlists, Artists
    }

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


    public void SelectTrack(Track track)
    {
        this.selectedTrack = track;
    }
}
