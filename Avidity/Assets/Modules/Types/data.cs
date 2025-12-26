using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

using Shard = System.String;


namespace Avidity
{
    [Serializable]
    public class ApplicationData
    {
        public Dictionary<Shard, Track>    tracks = new();
        public Dictionary<Shard, Playlist> playlists = new();
        public Dictionary<Shard, Artist>   artists = new();
    }

    internal record ApplicationDataExchange
    {
        public Dictionary<Shard, TrackDataExchange>    tracks = new();
        public Dictionary<Shard, PlaylistDataExchange> playlists = new();
        public Dictionary<Shard, Artist>               artists = new();
    }
}
