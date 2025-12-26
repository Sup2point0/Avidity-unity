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
    }
}
