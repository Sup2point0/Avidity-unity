#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Shard = System.String;


namespace Avidity
{
    /// <summary> A soundtrack with all its associated data. </summary>
    [Serializable]
    public class Track
    {
        /// <summary> Internal identifier of the track. </summary>
        public Shard? shard;

        /// <summary> The audio file name (if different to the track shard) of the track. </summary>
        public string? source;
        
        /// <summary> Exact displayed name of the track. </summary>
        public string? name;
        
        /// <summary> Artists assigned to the track. </summary>
        public List<Artist>? artists;

        /// <summary> Duration of the track in seconds. </summary>
        public float? duration;

        // TODO: Write custom serialiser
        /// <summary> Album the track belongs to. </summary>
        [NonSerialized] public Playlist? album;
        
        /// <summary> Playlists the track belongs to. </summary>
        [NonSerialized] public List<Playlist>? playlists;
        
        /// <summary> Total number of times the track has been played. </summary>
        public int totalPlays = 0;
        
        
        public string DisplayName()
            => this.name ?? "Untitled Track";

        public string DisplayDuration()
        {
            if (this.duration is null) return "--:--";

            var seconds = (int) this.duration;

            var mins = seconds / 60;
            var m = mins.ToString();

            var secs = seconds % 60;
            var s = secs.ToString().PadLeft(2, '0');

            return $"{m}:{s}";
        }
    }


    /// <summary> Intermediate object for converting between a `Track` and JSON. </summary>
    public record TrackDataExchange
    {
        public string?      name;
        public string?      source;
        public List<Shard>? artists;
        public float?       duration;
        public Shard?       album;
        public List<Shard>? playlists;
        public int          totalPlays = 0;


        public Track? ToTrack(Shard shard, Avidity.ApplicationData data)
        {
            try {
                return new Track() {
                    shard  = shard,
                    source = this.source,
                    name   = this.name,

                    artists =
                        (this.artists is null) ? null : (
                            from artist_shard in this.artists
                            where data.artists.ContainsKey(artist_shard)
                            select data.artists[artist_shard]
                        ).ToList(),
                    
                    duration = this.duration,

                    album =
                        (this.album is null) ? null
                        : data.playlists[this.album],
                    
                    playlists =
                        (this.playlists is null) ? null : (
                            from list_shard in this.playlists 
                            where data.playlists.ContainsKey(list_shard)
                            select data.playlists[list_shard]
                        ).ToList(),
                    
                    totalPlays = this.totalPlays,
                };
            }
            catch (Exception e) {
                Debug.LogError($"Error loading track {shard}: {e}");
                return null;
            }
        }
    }
}
