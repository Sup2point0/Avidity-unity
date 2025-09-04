using System;

using UnityEngine;


namespace Avidity
{
    [CreateAssetMenu(fileName = "colours", menuName = "Scriptable Objects/Colour Palette")]
    public class ColourPalette : ScriptableObject
    {
        public BackgroundColours Back;

        [Serializable]
        public class BackgroundColours
        {
            public UnityEngine.Color Protive;
            public UnityEngine.Color Deutive;
            public UnityEngine.Color Tritive;
        }
    }
}
