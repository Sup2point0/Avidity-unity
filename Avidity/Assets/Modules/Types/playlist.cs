#nullable enable

using System;
using System.Collections.Generic;

using UnityEngine;

using Shard = System.String;


namespace Avidity
{
    /// <summary> A playlist with all its associated data. </summary>
    [Serializable]
    public class Playlist : Avidity.ISelectableObject
    {
        /// <summary> Base number of 'phantom' plays to add when randomly selecting tracks. </summary>
        private const int BASE_PLAYS = 69;


        /// <summary> Internal identifier of the playlist. </summary>
        public Shard? shard;

        /// <summary> Exact displayed name of the playlist. </summary>
        public string? name;
        
        /// <summary> Tracks in the playlist. </summary>
        public List<Track>? tracks;

        public PlaylistKind? kind;

        /// <summary> Accent colour of the playlist. </summary>
        public UnityEngine.Color? colour;

        /// <summary> Is this playlist an album? </summary>
        public bool isAlbum = false;

        /// <summary> Total number of times tracks in this playlist have been played. </summary>
        public int totalPlays = 0;


        /// <summary> The first track in the playlist, or <c>null</c> if the playlist is empty. </summary>
        public Track? firstTrack => (this.tracks?.Count > 0) ? this.tracks[0] : null;


        public string DisplayName()
            => this.name ?? "Untitled Playlist";


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
    }


    public record PlaylistKind
    {
#pragma warning disable CS8618

        public Shard  shard { get; init; }
        public string text  { get; init; }

#pragma warning restore CS8618

        public static PlaylistKind Album = new() { shard = "album", text = "Album" };
        public static PlaylistKind Genre = new() { shard = "genre", text = "Genre" };
        public static PlaylistKind Mood  = new() { shard = "mood",  text = "Mood"  };
        public static PlaylistKind Label = new() { shard = "label", text = "Label" };

        private static readonly Dictionary<Shard, PlaylistKind> KINDS = new() {
            { "album", Album },
            { "genre", Genre },
            { "mood" , Mood },
            { "label", Label },
        };

        public static PlaylistKind? FromString(Shard shard)
            => KINDS.TryGetValue(shard, out var kind) ? kind : null;
    }


    /// <summary> Intermediate object for converting between a `Playlist` and JSON. </summary>
    public record PlaylistDataExchange
    {
        public string?      name;
        public List<Shard>? tracks;
        public Shard?       kind;
        public string?      colour;
        public bool         isAlbum = false;
        public int          totalPlays = 0;


        public Playlist? ToInitialisedPlaylist(Shard shard)
        {
            try {
                return new Playlist() {
                    shard  = shard,
                    name   = this.name,
                    tracks = new(),

                    kind =
                        (this.kind is null) ? null
                        : PlaylistKind.FromString(this.kind),

                    colour =
                        (this.colour is null) ? null
                        : ColorUtility.TryParseHtmlString(this.colour, out var colour)
                        ? colour : null,
                    
                    isAlbum = this.isAlbum,
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
