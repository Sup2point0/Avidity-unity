using System.Collections.Generic;
using System.IO;

using UnityEngine;


namespace Avidity
{
    /// <summary> Executive for managing data persistence (loading and saving). </summary>
    public static class Persistence
    {
        public static Avidity.ApplicationOptions options;
        public static Avidity.ApplicationData    data;


    #region STATIC

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadData()
        {
            LoadFromPath(Application.persistentDataPath);
        }

    #endregion


    #region INITIALISERS

        public static void LoadFromPath(string source_path)
        {
            LoadOptions(source_path);
            LoadData(source_path);
        }

    #endregion


    #region OPTIONS LOADING

        public static void LoadOptions(string source_path)
        {
            Persistence.options = LoadJson<Avidity.ApplicationOptions>($"{source_path}/options.json");
        }

    #endregion


    #region DATA LOADING

        public static void LoadData(string source_path)
        {
            Persistence.data = new();
            LoadTracks($"{source_path}/tracks.json");
            LoadPlaylists($"{source_path}/playlists.json");
            LoadArtists($"{source_path}/artists.json");
        }

        public static void LoadTracks(string path)
        {
            Persistence.data.tracks = LoadJson<List<Track>>(path);
        }
        
        public static void LoadPlaylists(string path) {}
        
        public static void LoadArtists(string path) {}

    #endregion


    #region SAVING

        public static void SaveOptions()
        {
            var data = JsonUtility.ToJson(Persistence.options);
            SaveFileTo("options.json", data);
        }

    #endregion


    #region INTERNAL

        private static T LoadJson<T>(string path)
        {
            var text = File.ReadAllText(path);
            var res = JsonUtility.FromJson<T>(text);

            return res;
        }

        private static void SaveFileTo(string path, string content)
        {
            var dest = $"{Application.persistentDataPath}/{path}";

            using FileStream stream = new(dest, FileMode.Create);
            using StreamWriter writer = new(stream);
            
            writer.Write(content);
        }

    #endregion


        // TODO:
        public static void LoadTracks()
        {
            data.tracks = new() {
                new Track() {
                    shard = "voxel", name = "Voxel", artists = new() { new Artist() { shard = "sup", name = "Sup#2.0", }, }, album = new() { shard = "cortex", name = "Cortex", },
                },
                new Track() {
                    shard = "colour-mourning", name = "Colour Mourning", artists = new() { new Artist() { shard = "camellia", name = "Camellia", }, },
                },
                new Track() {
                    shard = "synthesis", name = "Synthesis.", artists = new() { new Artist() { shard = "tn-shi", name = "tn-shi", }, },
                },
                new Track() {
                    shard = "synthesis", name = "Synthesis.", artists = new() { new Artist() { shard = "tn-shi", name = "tn-shi", }, },
                },
                new Track() {
                    shard = "synthesis", name = "Synthesis.", artists = new() { new Artist() { shard = "tn-shi", name = "tn-shi", }, },
                },
                new Track() {
                    shard = "synthesis", name = "Synthesis.", artists = new() { new Artist() { shard = "tn-shi", name = "tn-shi", }, },
                },
                new Track() {
                    shard = "terabyte", name = "TERABYTE CONNECTION", artists = new() { new Artist() { shard = "camellia", name = "Camellia", }, }, album = new() { shard = "tera-io", name = "Tera I/O", },
                },
                new Track() {
                    shard = "synthesis", name = "Synthesis.", artists = new() { new Artist() { shard = "tn-shi", name = "tn-shi", }, },
                },
                new Track() {
                    shard = "terabyte", name = "TERABYTE CONNECTION", artists = new() { new Artist() { shard = "camellia", name = "Camellia", }, }, album = new() { shard = "tera-io", name = "Tera I/O", },
                },
            };
        }
    }
}
