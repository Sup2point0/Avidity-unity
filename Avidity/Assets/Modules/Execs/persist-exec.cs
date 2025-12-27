using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using UnityEngine;

using Shard = System.String;


namespace Avidity
{
    /// <summary> Executive for managing data persistence (loading and saving). </summary>
    public static class Persistence
    {
        public static Avidity.ApplicationOptions options;
        public static Avidity.ApplicationData    data;


    #region STATIC

        /// <summary> Called when the application first starts, to load all the data before the UI displays. </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadData()
        {
            LoadFromPath(Application.persistentDataPath);
        }

    #endregion


    #region INITIALISERS

        public static void LoadFromPath(string source_path)
        {
            Persistence.options = LoadOptions(source_path);
            Persistence.data    = LoadData(source_path);
        }

    #endregion


    #region OPTIONS LOADING

        public static Avidity.ApplicationOptions LoadOptions(string source_path)
        {
            return LoadJson<Avidity.ApplicationOptions>($"{source_path}/options.json");
        }

    #endregion


    #region DATA LOADING

        public static Avidity.ApplicationData LoadData(string source_path)
        {
            /* First, load data from JSON into intermediate dictionaries */
            var exchange = new Avidity.ApplicationDataExchange();

            exchange.tracks    = LoadTracksExchange($"{source_path}/tracks.json");
            exchange.playlists = LoadPlaylistsExchange($"{source_path}/playlists.json");
            exchange.artists   = LoadArtists($"{source_path}/artists.json");

            /* Then resolve shard references to actual objects */
            var data = new Avidity.ApplicationData();
            
            data.artists   = InitArtists(exchange);
            data.playlists = InitPlaylists(exchange);

            data.tracks    = LinkTracks(exchange.tracks, data);
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

        private static Dictionary<Shard, Playlist> InitPlaylists(Avidity.ApplicationDataExchange data)
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
            Avidity.ApplicationData data
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
            Avidity.ApplicationData data
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

        private static Dictionary<Shard, Artist> InitArtists(Avidity.ApplicationDataExchange data)
            => (from kvp in data.artists
                let shard = kvp.Key
                let artist_data = kvp.Value
                let artist = artist_data.ToArtist(shard)
                where artist is not null
                select artist
            ).ToDictionary(artist => artist.shard, artist => artist);

    #endregion


    #region SAVING

        public static void SaveOptions()
        {
            SaveJson(Persistence.options, "options.json");
        }

    #endregion
    #region INTERNAL

        private static T LoadJson<T>(string path)
        {
            var text = File.ReadAllText(path);
            var res = JsonConvert.DeserializeObject<T>(text, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return res;
        }

        private static void SaveJson<T>(T obj, string path)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
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
