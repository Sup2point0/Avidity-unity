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


    void Start()
    {
    }

    void Update()
    {
    }
}
