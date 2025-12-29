#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Avid = Avidity;

using Shard = System.String;


namespace Avidity
{
    /// <summary> An artist with all its associated data. </summary>
    [Serializable]
    public class Artist : Bases.ISelectableEntity
    {
        /// <summary> Shard (primary key) of the artist. </summary>
        /// <remarks>
        /// Guaranteed to be non-null, has to be nullable due to lack of <c>required</c> in C# 9.0.
        /// </remarks>
        public Shard? shard;

        /// <summary> Exact displayed name of the artist. </summary>
        public string? name;

        /// <summary> Folder where the artist's tracks are stored. </summary>
        public string? folder;

        /// <summary> Total number of tracks by the artist. </summary>
        /// <remarks>
        /// This is dynamically re-calculated on application startup, not loaded from persistent data.
        /// </remarks>
        public int totalTracks = 0;

        /// <summary> Total number of times tracks by the artist have been played. </summary>
        public int totalPlays = 0;


        public Artist ExportJson()
            => new() {
                name        = this.name,
                folder      = this.folder,
                totalTracks = this.totalTracks,
                totalPlays  = this.totalPlays,
            };


        /// <summary> Find the folder where we expect the artist's tracks to be stored. </summary>
        public string? ResolveFolder()
            => this.folder ?? this.shard ?? this.name?.ToLower() ?? null;


        public string DisplayName()
            => this.name ?? $"Unnamed [{this.shard}]";

        public static string DisplayNames(IEnumerable<Artist>? artists)
            => (artists is null) ? "Anonymous"
                : string.Join(" / ", artists.Select(each => each.DisplayName()));


        public override bool Equals(object obj)
            => (obj is Artist) && this.shard!.Equals((obj as Artist)!.shard);

        public override int GetHashCode()
            => this.shard!.GetHashCode();
    }

    
    /// <summary> Intermediate object for converting between a `Artist` and JSON. </summary>
    public record ArtistDataExchange
    {
        public string? name;
        public string? folder;
        public int     totalPlays = 0;


        public Artist? ToArtist(Shard shard)
        {
            try {
                return new Artist() {
                    shard      = shard,
                    name       = this.name,
                    folder     = this.folder,
                    totalPlays = this.totalPlays,
                };
            }
            catch (Exception e) {
                Debug.LogError($"Error loading artist `{shard}`: {e}");
                return null;
            }
        }
    }
}
