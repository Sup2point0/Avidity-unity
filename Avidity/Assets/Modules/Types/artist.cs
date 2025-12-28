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
    public class Artist : Bases.ISelectableObject
    {
        /// <summary> Internal identifier of the artist. </summary>
        public Shard? shard;

        /// <summary> Exact displayed name of artist. </summary>
        public string? name;

        public string? folder;

        /// <summary> Total number of times tracks by this artist have been played. </summary>
        public int totalPlays = 0;


        public Artist ExportJson()
            => new() {
                name       = this.name,
                folder     = this.folder,
                totalPlays = this.totalPlays,
            };


        /// <summary> Find the folder where we expect the artist's tracks to be stored. </summary>
        public string? ResolveFolder()
            => this.folder ?? this.shard ?? this.name?.ToLower() ?? null;


        public string DisplayName()
            => this.name ?? "Anonymous";

        public static string DisplayNames(IEnumerable<Artist>? artists)
            => (artists is null) ? "Anonymous"
                : string.Join(" / ", artists.Select(each => each.DisplayName()));
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
