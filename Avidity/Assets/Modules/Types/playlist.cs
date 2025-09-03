using System.Collections.Generic;

using Shard = System.String;


/// <summary> A playlist with all its associated data. </summary>
public class Playlist
{
    /// <summary> Internal identifier of the playlist. </summary>
    public Shard shard { get; init; }

    /// <summary> Exact displayed name of the playlist. </summary>
    public string name { get; set; }
    
    /// <summary> Tracks in the playlist. </summary>
    public List<Track> tracks { get; set; }

    /// <summary> Filename of the cover image for the playlist. </summary>
    public string cover_filename { get; set; }

    /// <summary> Accent colour of the playlist. </summary>
    public UnityEngine.Color colour { get; set; }

    /// <summary> Is this playlist an album? </summary>
    public bool is_album { get; set; } = false;

    /// <summary> Total number of times tracks in this playlist have been played. </summary>
    public int total_plays { get; set; }


    /// <summary> The first track in the playlist, or <c>null</c> if the playlist is empty. </summary>
    public Track first_track => (tracks.Count > 0) ? tracks[0] : null;
}
