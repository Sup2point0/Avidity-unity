using System;
using System.Collections.Generic;
using System.Linq;

using Shard = System.String;


/// <summary> A playlist with all its associated data. </summary>
[Serializable]
public class Playlist
{
    /// <summary> Base number of 'phantom' plays to add when randomly selecting tracks. </summary>
    private const int BASE_PLAYS = 69;


    /// <summary> Internal identifier of the playlist. </summary>
    public Shard shard;

    /// <summary> Exact displayed name of the playlist. </summary>
    public string name;
    
    /// <summary> Tracks in the playlist. </summary>
    public List<Track> tracks;

    /// <summary> Accent colour of the playlist. </summary>
    public UnityEngine.Color colour;

    /// <summary> Is this playlist an album? </summary>
    public bool isAlbum = false;

    /// <summary> Total number of times tracks in this playlist have been played. </summary>
    public int totalPlays;


    /// <summary> The first track in the playlist, or <c>null</c> if the playlist is empty. </summary>
    public Track firstTrack => (tracks.Count > 0) ? tracks[0] : null;


    public static implicit operator string(Playlist playlist)
        => playlist.ToString();

    public override string ToString()
        => this.name;


    // public List<Track> GetShuffledList()
    // {
    //     int highest_plays = (
    //         from each in this.tracks
    //         select each.totalPlays
    //     ).Max();

    //     WeightedList<Track> pool = new(
    //         from each in this.tracks select
    //         (BASE_PLAYS + highest_plays - each.totalPlays,
    //          each)
    //     );

    //     return pool.GetRandomItems(count: this.tracks.Count, drop: true);
    // }
}
