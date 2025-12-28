#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using UnityEngine;

using Avidity;
using Avid = Avidity;

using Shard = System.String;
using Newtonsoft.Json.Serialization;


namespace Avidity
{
    /// <summary> A soundtrack with all its associated data. </summary>
    /// <remarks>
    /// We're modelling tracks as a 'weak entity': we can't reliably assign them a unique shard based no their name alone, since multiple artists could create tracks with the same name. Instead, we'll compute a 'composite shard' by joining their primary artist's shard and their own weak shard. We'll use the composite shard to index them globally, but still keep hold of the weak shard to have a filename to automatically look for when playing audio.
    /// </remarks>
    [Serializable]
    public class Track : Avid.ISelectableObject
    {
    
    #region TECHNICAL

        /// <summary> Global shard of the track, composed from its primary artist's shard and its weak shard. `null` if the track has no weak shard or no attributed artists. </summary>
        public Shard? shard =>
            (this.weak_shard is null && this.primary_artist_shard is null) ? null
            : $"{this.primary_artist_shard}-{this.weak_shard}";

        /// <summary> Internal identifier of the track. </summary>
        public Shard? weak_shard;

        /// <summary> Internal identifier of the track's primary artist. </summary>
        private Shard? primary_artist_shard {
            get {
                this._artist_shard ??= this.ResolvePrimaryArtist()?.shard;
                return this._artist_shard;
            }
        }
        private Shard? _artist_shard;

        /// <summary> The audio file name (if different to the track shard) of the track. </summary>
        public string? audio_file;

        /// <summary> The audio file extension of the track. </summary>
        public TrackAudioFileKind filetype = TrackAudioFileKind.MP3;

        /// <summary> Duration of the track in seconds. </summary>
        public float? duration;
    
    #endregion
    #region NON-TECHNICAL

        /// <summary> Exact displayed name of the track. </summary>
        public string? name;
        
        /// <summary> Artists assigned to the track. </summary>
        public List<Artist>? artists;

        /// <summary> Album the track belongs to. </summary>
        [NonSerialized] public Playlist? album;
        
        /// <summary> Playlists the track belongs to. </summary>
        [NonSerialized] public List<Playlist>? playlists;
        
        /// <summary> Total number of times the track has been played. </summary>
        public int totalPlays = 0;
    
    #endregion


        public Dictionary<string, object> ExportJson()
        {
            Dictionary<string, object> res = new();

            Utils.AddMaybe(res, "weak-shard", this.weak_shard);
            Utils.AddMaybe(res, "name",       this.name);
            Utils.AddMaybe(res, "audio",      this.audio_file);
            Utils.AddMaybe(res, "duration",   this.duration);
            Utils.AddMaybe(res, "artists",    this.artists?.Select(artist => artist.shard).ToList());
            Utils.AddMaybe(res, "album",      this.album?.shard);
            Utils.AddMaybe(res, "playlists",  this.playlists?.Select(list => list.shard)).ToList();
            Utils.AddMaybe(res, "plays",      this.totalPlays);

            return res;
        }
        

        /// <summary> Find the name of the file in which we expect the track's audio to be stored. </summary>
        public string? ResolveSource()
            => this.audio_file ?? this.weak_shard ?? this.name?.ToLower() ?? null;
        
        /// <summary> Find the primary artist the track should be tied to. </summary>
        public Artist? ResolvePrimaryArtist()
            => (this.artists is null) ? null
                : (this.artists.Count > 0) ? this.artists[0] : null;

        /// <summary> Find the folders in which we could expect the track's audio to be found. </summary>
        public List<string> ResolveFolders()
        {
            var track_file = this.ResolveSource();
            var ext = this.filetype.shard;

            return (
                from source_folder in Persistence.options.audio_source_folders
                let artist_folder = this.ResolvePrimaryArtist()?.ResolveFolder()
                let path = Utils.JoinPaths(source_folder, artist_folder, track_file)
                select $"file://{path}.{ext}"
            ).ToList();
        }
        

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


    public sealed record TrackAudioFileKind : Avid.Bases.SerialisedEnum<TrackAudioFileKind>
    {
        public static TrackAudioFileKind MP3 = new() { shard = "mp3", text = ".mp3" };
        public static TrackAudioFileKind M4A = new() { shard = "m4a", text = ".m4a" };
        public static TrackAudioFileKind WAV = new() { shard = "wav", text = ".wav"  };

        private static readonly Dictionary<Shard, TrackAudioFileKind> KINDS = new() {
            ["mp3"] = MP3,
            ["m4a"] = M4A,
            ["wav"] = WAV,
        };

        public static TrackAudioFileKind? FromString(Shard shard)
            => FromVariants(KINDS, shard);
    }


    /// <summary> Intermediate object for converting between a `Track` and JSON. </summary>
    // [JsonObject(NamingStrategyType = typeof(KebabCaseNamingStrategy))]
    public record TrackDataExchange
    {
        [JsonProperty("weak-shard")]
        public string?      weak_shard;
        public string?      name;
        public string?      audio;
        public float?       duration;
        public List<Shard>? artists;
        public Shard?       album;
        public List<Shard>? lists;
        public int          plays = 0;


        public Track? ToTrack(Shard shard, Avid.ApplicationData data)
        {
            try {
                return new Track() {
                    weak_shard = this.weak_shard,
                    audio_file = this.audio,
                    name       = this.name,

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
                        (this.lists is null) ? null : (
                            from list_shard in this.lists 
                            where data.playlists.ContainsKey(list_shard)
                            select data.playlists[list_shard]
                        ).ToList(),
                    
                    totalPlays = this.plays,
                };
            }
            catch (Exception e) {
                Debug.LogError($"Error loading track `{shard}`: {e}");
                return null;
            }
        }
    }
}
