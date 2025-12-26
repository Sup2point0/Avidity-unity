using System;
using System.Collections.Generic;

using UnityEngine;


namespace Avidity
{
    [Serializable]
    public class ApplicationOptions
    {
        public List<string> audioSourceFolders = new();

        public float volume = 1.0f;

        public SceneExecutive.Tab defaultTab;


        public void Save()
        {
            var data = JsonUtility.ToJson(this);
            Utils.SaveFile("options.json", data);
        }
    }
}
