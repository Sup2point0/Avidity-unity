#nullable enable

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


        /// <summary> Fetch an artist by its shard, or if no artist exists with that shard, create a new one. </summary>
        public Artist GetOrCreateArtist(Shard shard)
        {
            if (this.artists.TryGetValue(shard, out var artist)) return artist;

            var created = new Artist() {
                shard = shard,
            };
            this.artists.Add(shard, created);

            return created;
        }

        /// <summary> <c>GetOrCreateArtist()</c>, but increments the artist's track count by 1. </summary>
        public Artist GetOrCreateArtistForTrack(Shard shard)
        {
            var artist = GetOrCreateArtist(shard);            
            artist.totalTracks++;
            return artist;
        }

        public Playlist GetOrCreatePlaylist(Shard shard)
        {
            if (this.playlists.TryGetValue(shard, out var list)) return list;

            var created = new Playlist() {
                shard = shard,
            };
            this.playlists.Add(shard, created);

            return created;
        }
    }

    internal record ApplicationDataExchange
    {
        public Dictionary<Shard, TrackDataExchange>    tracks = new();
        public Dictionary<Shard, PlaylistDataExchange> playlists = new();
        public Dictionary<Shard, ArtistDataExchange>   artists = new();
    }
}
