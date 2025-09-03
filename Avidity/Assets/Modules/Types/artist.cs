using System.Collections.Generic;

using Shard = System.String;


/// <summary> An artist with all its associated data. </summary>
public class Artist
{
    /// <summary> Internal identifier of the artist. </summary>
    public Shard shard { get; init; }

    /// <summary> Exact displayed name of artist. </summary>
    public string name { get; init; }

    /// <summary> Filename of the cover image for the artist. </summary>
    public string cover_filename { get; init; }

    /// <summary> Total number of times tracks by this artist have been played. </summary>
    public int plays { get; set; }


    public override string ToString()
        => this.name;


    public static string DisplayNames(IEnumerable<Artist> artists)
    {
        return string.Join(" / ", artists);
    }
}
