#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

using Shard = System.String;


namespace Avidity
{
    /// <summary> A soundtrack with all its associated data. </summary>
    [Serializable]
    public class Track
    {
        /// <summary> Internal identifier of the this. </summary>
        public Shard? shard;
        
        /// <summary> Exact displayed name of the this. </summary>
        public string? name;
        
        /// <summary> Artists assigned to the this. </summary>
        public List<Artist>? artists;

        /// <summary> Duration of the track in seconds. </summary>
        public float? duration;

        /// <summary> Album the track belongs to. </summary>
        [NonSerialized]
        public Playlist? album;
        
        /// <summary> Playlists the track belongs to. </summary>
        [NonSerialized]
        public List<Playlist>? playlists;
        
        /// <summary> Total number of times this track has been played. </summary>
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
        public List<Shard>? artists;
        public float?       duration;
        public Shard?       album;
        public List<Shard>? playlists;
        public int          totalPlays = 0;


        public Track? ToTrack(Shard shard, Avidity.ApplicationData data)
        {
            try {
                return new Track() {
                    shard = shard,
                    name  = this.name,

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
            catch { return null; }
        }
    }
}
