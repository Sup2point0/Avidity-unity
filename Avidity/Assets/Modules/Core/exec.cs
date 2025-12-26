using System;

using UnityEngine;


namespace Avidity
{
    /// <summary> The global collection of Executive singletons. </summary>
    public static class Exec
    {
        /// <summary> Tried assigning to a singleton that already has a value. </summary>
        public class SingletonOverwriteException : Exception
        {
            public SingletonOverwriteException() {}
            public SingletonOverwriteException(string message) : base(message) {}
        }


        private static SceneExecutive scene_exec;
        private static AudioExecutive audio_exec;

        /// <summary> The scene executive for managing application state and navigation. </summary>
        public static SceneExecutive Scene {
            get => Exec.scene_exec;
            set {
                if (Exec.scene_exec == null) {
                    Exec.scene_exec = value;
                } else {
                    GameObject.Destroy(value);
                    throw new SingletonOverwriteException("Scene executive already exists!");
                }
            }
        }

        /// <summary> The audio executive for managing audio playback. </summary>
        public static AudioExecutive Audio {
            get => Exec.audio_exec;
            set {
                if (Exec.audio_exec == null) {
                    Exec.audio_exec = value;
                } else {
                    GameObject.Destroy(value);
                    throw new SingletonOverwriteException("Audio executive already exists!");
                }
            }
        }
    }
}
