using System;
using System.Collections.Generic;

using Shard = System.String;


/// <summary> A soundtrack with all its associated data. </summary>
[Serializable]
public class Track
{
    /// <summary> Internal identifier of the track. </summary>
    public Shard shard { get; init; }
    
    /// <summary> Exact displayed name of the track. </summary>
    public string name { get; set; }
    
    /// <summary> Artists assigned to the sountrack. </summary>
    public List<Artist> artists { get; init; }
    
    /// <summary> Playlists the track belongs to. </summary>
    public List<Playlist> playlists { get; set; }

    /// <summary> Filename of the audio file for the track. </summary>
    public string audio_filename { get; set; }

    /// <summary> Filename of the cover image for the track. </summary>
    public string cover_filename { get; set; }
    
    /// <summary> Total number of times this track has been played. </summary>
    public int total_plays { get; set; }
}
