#pragma warning disable IDE1006

using System;
using System.Collections.Generic;

using UnityEngine;

using Shard = System.String;


/// <summary> A soundtrack with all its associated data. </summary>
[Serializable]
public class Track
{
    /// <summary> Internal identifier of the soundtrack. </summary>
    public Shard shard { get; init; }
    
    /// <summary> Exact displayed name of the soundtrack. </summary>
    public string name { get; init; }
    
    /// <summary> Artists assigned to the sountrack. </summary>
    public List<Artist> artists { get; init; }
    
    /// <summary> Number of times this soundtrack has been played. </summary>
    public int plays { get; set; }
    
    /// <summary> Playlists the soundtrack belongs to. </summary>
    public List<Playlist> playlists { get; set; }
}
