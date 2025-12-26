using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;


namespace Avidity
{
    [Serializable]
    public class ApplicationData
    {
        public List<Track>    tracks = new();
        public List<Playlist> playlists = new();
        public List<Artist>   artists = new();


        public static ApplicationData LoadFromPath(string source_path)
        {
            return new ApplicationData().LoadData(source_path);
        }

        public ApplicationData LoadData(string source_path)
        {
            LoadTracks(source_path);
            LoadPlaylists(source_path);
            LoadArtists(source_path);
            return this;
        }

        public void LoadTracks(string source_path)
        {
            this.tracks = LoadJson<List<Track>>($"{source_path}/tracks.json");
        }
        
        public void LoadPlaylists(string source_path) {}
        
        public void LoadArtists(string source_path) {}

        private T LoadJson<T>(string path)
        {
            var text = File.ReadAllText(path);
            var res = JsonUtility.FromJson<T>(text);

            return res;
        }
    }
}
