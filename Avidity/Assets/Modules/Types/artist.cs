#nullable enable

using System;
using System.Collections.Generic;

using Shard = System.String;


/// <summary> An artist with all its associated data. </summary>
[Serializable]
public class Artist
{
    /// <summary> Internal identifier of the artist. </summary>
    public Shard? shard;

    /// <summary> Exact displayed name of artist. </summary>
    public string? name;

    /// <summary> Total number of times tracks by this artist have been played. </summary>
    public int totalPlays = 0;


    public string DisplayName()
        => this.name ?? "Anonymous";

    public static string DisplayNames(IEnumerable<Artist> artists)
        => string.Join(" / ", artists);
}
