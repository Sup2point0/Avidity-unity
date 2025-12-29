using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using UnityEngine;

using Avid = Avidity;


using Shard = System.String;


namespace Avidity
{
    /// <summary> Executive for managing data persistence (loading and saving). </summary>
    public static class Persistence
    {
        public static Avid.ApplicationOptions options;
        public static Avid.ApplicationData    data;

        private static readonly JsonSerializerSettings json_serialiser_settings = new() {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver() {
                NamingStrategy = new KebabCaseNamingStrategy()
            },
        };
        private static readonly JsonSerializerSettings json_deserialiser_settings = new() {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver() {
                NamingStrategy = new KebabCaseNamingStrategy()
            },
        };


    #region INITIALISERS

        /// <summary> Called when the application first starts, to load all the data before the UI displays. </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadData()
        {
            var source_path = Application.persistentDataPath;

            Persistence.options = LoadOptions(source_path);
            Persistence.data    = LoadData(source_path);
        }

    #endregion


    #region OPTIONS LOADING

        public static Avid.ApplicationOptions LoadOptions(string source_path)
        {
            return LoadJson<Avid.ApplicationOptions>($"{source_path}/options.json");
        }

    #endregion


    #region DATA LOADING

        public static Avid.ApplicationData LoadData(string source_path)
        {
            /* First, load data from JSON into intermediate dictionaries */
            var exchange = new Avid.ApplicationDataExchange();

            exchange.artists   = LoadArtists($"{source_path}/artists.json");
            exchange.tracks    = LoadTracksExchange($"{source_path}/tracks.json");
            exchange.playlists = LoadPlaylistsExchange($"{source_path}/playlists.json");

            /* Then resolve shard references to actual objects */
            var data = new Avid.ApplicationData();
            
            data.artists   = InitArtists(exchange);
            data.playlists = InitPlaylists(exchange);

            data.tracks = LinkTracks(exchange.tracks, data);
            LinkPlaylists(exchange.playlists, data);

            return data;
        }


        private static Dictionary<Shard, TrackDataExchange> LoadTracksExchange(string path)
            => LoadJson<Dictionary<Shard, TrackDataExchange>>(path);
        
        private static Dictionary<Shard, PlaylistDataExchange> LoadPlaylistsExchange(string path)
            => LoadJson<Dictionary<Shard, PlaylistDataExchange>>(path);
        
        private static Dictionary<Shard, ArtistDataExchange> LoadArtists(string path)
            => LoadJson<Dictionary<Shard, ArtistDataExchange>>(path);

    #endregion
    #region DATA LINKING

        private static Dictionary<Shard, Playlist> InitPlaylists(Avid.ApplicationDataExchange data)
            => (from kvp in data.playlists
                let shard = kvp.Key
                let list_data = kvp.Value
                let list = list_data.ToInitialisedPlaylist(shard)
                where list is not null
                select list
            ).ToDictionary(list => list.shard, list => list);


        /// <summary> Replace Shards in partially initialised track objects to the objects they represent (pure). </summary>
        /// <param name="tracks">Tracks to link.</param>
        /// <param name="data">Data proving objects to link to.</param>
        /// <returns>Fully initialised tracks with Shards now linked.</returns>
        private static Dictionary<Shard, Track> LinkTracks(
            Dictionary<Shard, TrackDataExchange> tracks,
            Avid.ApplicationData data
        )
            => (from kvp in tracks
                let shard = kvp.Key
                let track_data = kvp.Value
                let track = track_data.ToTrack(shard, data)
                where track is not null
                select track
            ).ToDictionary(track => track.shard, track => track);


        /// <summary> Replace Shards in playlist objects to the objects they represent (in-place). </summary>
        /// <param name="playlists">Playlists to link.</param>
        /// <param name="data">Data providing objects to link to.</param>
        private static void LinkPlaylists(
            Dictionary<Shard, PlaylistDataExchange> playlists,
            Avid.ApplicationData data
        )
        {
            foreach (var kvp in playlists) {
                var list_shard = kvp.Key;
                var list = kvp.Value;

                if (list.tracks is null) continue;

                data.playlists[list_shard].tracks = (
                    from shard_track in list.tracks
                    where data.tracks.ContainsKey(shard_track)
                    select data.tracks[shard_track]
                ).ToList();
            }
        }

        private static Dictionary<Shard, Artist> InitArtists(Avid.ApplicationDataExchange data)
            => (from kvp in data.artists
                let shard = kvp.Key
                let artist_data = kvp.Value
                let artist = artist_data.ToArtist(shard)
                where artist is not null
                select artist
            ).ToDictionary(artist => artist.shard, artist => artist);

    #endregion


    #region OPTIONS SAVING

        public static void SaveOptions()
        {
            SaveJson(Persistence.options, "options.json");
        }

    #endregion


    #region DATA SAVING

        public static void SaveData()
        {
            SaveArtists();
            SaveTracks();
            SavePlaylists();
        }

        public static void SaveArtists()
        {
            var export = Persistence.data.artists.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.ExportJson()
            );

            SaveJson(export, "artists-export.json");
        }

        public static void SaveTracks()
        {
            var export = Persistence.data.tracks.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.ExportJson()
            );

            SaveJson(export, "tracks-export.json");
        }

        public static void SavePlaylists()
        {
            var export = Persistence.data.playlists.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.ExportJson()
            );

            SaveJson(export, "playlists-export.json");
        }


    #endregion


    #region INTERNAL

        private static T LoadJson<T>(string path)
        {
            var text = File.ReadAllText(path);
            var res = JsonConvert.DeserializeObject<T>(text, Persistence.json_deserialiser_settings);

            return res;
        }

        private static void SaveJson<T>(T obj, string path)
        {
            var json = JsonConvert.SerializeObject(obj, Persistence.json_serialiser_settings);
            SaveFile(json, path);
        }

        private static void SaveFile(string content, string path)
        {
            var dest = $"{Application.persistentDataPath}/{path}";

            using FileStream stream = new(dest, FileMode.Create);
            using StreamWriter writer = new(stream);
            
            writer.Write(content);
        }

    #endregion

    }
}
