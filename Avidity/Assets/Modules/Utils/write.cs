using System.IO;

using UnityEngine;


namespace Avidity
{
    public static partial class Utils
    {
        public static void SaveFile(string path, string content)
        {
            var dest = $"{Application.persistentDataPath}/{path}";

            using FileStream stream = new(dest, FileMode.Create);
            using StreamWriter writer = new(stream);
            
            writer.Write(content);
        }
    }
}
