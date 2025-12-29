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

            InitAndLinkTracks(exchange.tracks, data);
            // LinkPlaylists(exchange.playlists, data);

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

        private static Dictionary<Shard, Artist> InitArtists(Avid.ApplicationDataExchange data)
            => (from kvp in data.artists
                let shard = kvp.Key
                let artist_data = kvp.Value

                let artist = artist_data.ToArtist(shard)
                where artist is not null
                select artist
            ).ToDictionary(artist => artist.shard, artist => artist);

        private static Dictionary<Shard, Playlist> InitPlaylists(Avid.ApplicationDataExchange data)
            => (from kvp in data.playlists
                let shard = kvp.Key
                let list_data = kvp.Value

                let list = list_data.ToInitialisedPlaylist(shard)
                where list is not null
                select list
            ).ToDictionary(list => list.shard, list => list);

        /// <summary> (in-place) Replace Shards in partially initialised track objects to the objects they represent, and add the tracks to the playlists they're in. </summary>
        /// <param name="tracks_data">Partially initialised tracks to link.</param>
        /// <param name="data">Data proving objects to link to.</param>
        /// <returns>Fully initialised tracks with Shards now linked.</returns>
        private static void InitAndLinkTracks(
            Dictionary<Shard, TrackDataExchange> tracks_data,
            Avid.ApplicationData data
        )
        {
            data.tracks = tracks_data
                .Select(kvp => {
                    var shard = kvp.Key;
                    var track_data = kvp.Value;

                    var track = track_data.ToTrack(shard, data);
                    if (track is null) {
                        Debug.Log("NULL TRACK");return null; }

                    if (track_data.album is not null) {
                        data.GetOrCreatePlaylist(track_data.album).tracks.Add(track);
                    }
                    foreach (string list_shard in track_data?.lists ?? new()) {
                        data.GetOrCreatePlaylist(list_shard).tracks.Add(track);
                    }
                    
                    return track;
                })
                .Where(t => t is not null)
                .ToDictionary(track => track.shard, track => track)
            ;
        }

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
