#pragma warning disable IDE1006

using System.Collections.Generic;

using Shard = System.String;


public class Artist
{
    public Shard shard { get; init; }

    public string name { get; init; }

    public string cover_filename { get; init; }

    public int plays { get; set; }
}
