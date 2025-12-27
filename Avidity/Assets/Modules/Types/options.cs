using System;
using System.Collections.Generic;

using UnityEngine;


namespace Avidity
{
    [Serializable]
    public class ApplicationOptions
    {
        public List<string> audio_source_folders = new() {
            "C:/Users/sup/Desktop/assets/sounds",
            "C:/Users/sup/Desktop/assets/sounds/#misc",
        };

        public float volume = 1.0f;

        public SceneExecutive.Tab default_tab = SceneExecutive.Tab.Tracks;
    }
}
