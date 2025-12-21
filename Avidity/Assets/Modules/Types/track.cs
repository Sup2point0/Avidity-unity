using System;
using System.Collections.Generic;

using Shard = System.String;


/// <summary> A soundtrack with all its associated data. </summary>
[Serializable]
public class Track
{
    /// <summary> Internal identifier of the track. </summary>
    public Shard shard;
    
    /// <summary> Exact displayed name of the track. </summary>
    public string name;
    
    /// <summary> Artists assigned to the track. </summary>
    public List<Artist> artists;

    /// <summary> Duration of the track in seconds. </summary>
    public float? duration;

    /// <summary> Album the track belongs to. </summary>
    [NonSerialized]
    public Playlist album;
    
    /// <summary> Playlists the track belongs to. </summary>
    [NonSerialized]
    public List<Playlist> playlists;
    
    /// <summary> Total number of times this track has been played. </summary>
    public int totalPlays;
    

    public string DisplayDuration()
    {
        if (this.duration is null) return "--:--";

        var mins = this.duration / 60;
        var m = mins.ToString();

        var secs = this.duration % 60;
        var s = secs.ToString().PadLeft(2, '0');

        return $"{m}:{s}";
    }


    public static implicit operator string(Track track)
        => track.ToString();

    public override string ToString()
        => this.name;
}
