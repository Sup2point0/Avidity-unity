#pragma warning disable IDE1006

using System.Collections.Generic;

using Shard = System.String;


/// <summary> A playlist with all its associated data. </summary>
public class Playlist
{
    /// <summary> Permanent unique internal identifier of the playlist. </summary>
    public Shard shard;
    
    /// <summary> . </summary>
    public List<Track> tracks;


    public Playlist()
    {
        
    }
}
