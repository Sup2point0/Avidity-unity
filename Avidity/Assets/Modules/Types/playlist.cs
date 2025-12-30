#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Avid = Avidity;

using Shard = System.String;


namespace Avidity
{
    /// <summary> A playlist with all its associated data. </summary>
    [Serializable]
    public class Playlist : Bases.ISelectableEntity
    {

    #region DATA

        /// <summary> Shard (primary key) of the playlist. </summary>
        /// <remarks>
        /// Guaranteed to be non-null, has to be nullable due to lack of <c>required</c> in C# 9.0.
        /// </remarks>
        public Shard? shard;

        /// <summary> Exact displayed name of the playlist. </summary>
        public string? name;

        public PlaylistKind? kind;

        public string? cover_file;
        
        /// <summary> Tracks in the playlist (unordered). </summary>
        public HashSet<Track> tracks = new();

        /// <summary> Accent colour of the playlist. </summary>
        public UnityEngine.Color? colour;

        /// <summary> Total number of times tracks in this playlist have been played. </summary>
        public int totalPlays = 0;

    #endregion


    #region COMPUTED

        public int trackCount => this.tracks?.Count ?? 0;

    #endregion


    #region EXPORTING

        public Dictionary<string, object> ExportJson()
        {
            Dictionary<string, object> res = new();

            Utils.AddMaybe(res, "name",   this.name);
            Utils.AddMaybe(res, "kind",   this.kind?.shard);
            Utils.AddMaybe(res, "tracks", this.tracks.Select(track => track.shard).ToList());
            Utils.AddMaybe(res, "cover",  this.cover_file?.ToString());
            Utils.AddMaybe(res, "col",    this.colour?.ToString());
            Utils.AddMaybe(res, "plays",  this.totalPlays);

            return res;
        }

    #endregion


    #region INTERNAL

        public List<Artist>? ResolvePrimaryArtists()
        {
            var artists = new MultiSet<Artist>();

            foreach (var track in this.tracks) {
                foreach (var artist in track.artists ?? new()) {
                    artists.Add(artist);
                }

                if (artists.Count > 5) break;
            }

            var res = artists.SortedDescending();
            return res;
        }

        public float? FindMinimumDuration(out bool may_exceed)
        {
            var did_encounter_null = false;

            var res = this.tracks.Sum(track => {
                if (track.duration is null) {
                    did_encounter_null = true;
                }
                return track.duration;
            });

            may_exceed = did_encounter_null;
            return res;
        }

    #endregion


    #region DISPLAYING

        public string DisplayName()
            => this.name ?? $"Untitled <{this.shard}>";

        public string DisplayArtists()
        {
            var artists = this.ResolvePrimaryArtists();
            if (artists is null) return "Unknown Artists";

            if (artists.Count > 3) {
                return $"{string.Join(", ", artists.Take(3))}, and {artists.Count - 3} more";
            } else {
                return string.Join(", ", artists);
            }
        }

        // TODO: Maybe need to make async>
        public string DisplayDuration()
            => Utils.Display.Timespan(this.FindMinimumDuration(out var may_exceed), may_exceed);


        // public List<Track> GetShuffledList()
        // {
        //     int highest_plays = (
        //         from each in this.tracks
        //         select each.totalPlays
        //     ).Max();

        //     WeightedList<Track> pool = new(
        //         from each in this.tracks select
        //         (BASE_PLAYS + highest_plays - each.totalPlays,
        //          each)
        //     );

        //     return pool.GetRandomItems(count: this.tracks.Count, drop: true);
        // }

    #endregion


    #region

        public override bool Equals(object obj)
            => (obj is Playlist) && this.shard!.Equals((obj as Playlist)!.shard);

        public override int GetHashCode()
            => this.shard!.GetHashCode();

    #endregion

    }


    public sealed record PlaylistKind : Bases.SerialisedEnum<PlaylistKind>
    {
        public static PlaylistKind ALBUM = new() { shard = "album", text = "Album" };
        public static PlaylistKind GENRE = new() { shard = "genre", text = "Genre" };
        public static PlaylistKind VIBE  = new() { shard = "vibe",  text = "Vibe"  };
        public static PlaylistKind LABEL = new() { shard = "label", text = "Label" };

        private static readonly Dictionary<Shard, PlaylistKind> KINDS = new() {
            ["album"] = ALBUM,
            ["genre"] = GENRE,
            ["vibe" ] = VIBE,
            ["label"] = LABEL,
        };

        public static PlaylistKind? FromString(Shard shard)
            => FromVariants(KINDS, shard);
    }


    /// <summary> Intermediate object for converting between a `Playlist` and JSON. </summary>
    public record PlaylistDataExchange
    {
        public string?      name;
        public Shard?       kind;
        public List<Shard>? tracks = new();
        public string?      cover;
        public string?      col;
        public int          totalPlays = 0;


        public Playlist? ToInitialisedPlaylist(Shard shard)
        {
            try {
                return new Playlist() {
                    shard  = shard,
                    name   = this.name,

                    kind =
                        (this.kind is null) ? null
                        : PlaylistKind.FromString(this.kind),
                    
                    cover_file = this.cover,
                    tracks     = new(),

                    colour =
                        (this.col is null) ? null
                        : ColorUtility.TryParseHtmlString(this.col, out var colour)
                        ? colour : null,
                    
                    totalPlays = this.totalPlays,
                };
            }
            catch (Exception e) {
                Debug.LogError($"Error loading playlist {shard}: {e}");
                return null;
            }
        }
    }
}
