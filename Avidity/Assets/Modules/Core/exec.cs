using System;

using UnityEngine;


/// <summary>
/// The global collection of executive singletons.
/// </summary>
public static class Exec
{
    /// <summary> Tried assigning to a singleton that already has a value. </summary>
    public class SingletonOverwriteException : Exception
    {
        public SingletonOverwriteException()
        {}

        public SingletonOverwriteException(string message) : base(message)
        {}
    }


    /// <summary> The scene executive for managing navigation. </summary>
    public static SceneExec Scene {
        get => scene_exec;
        set {
            if (scene_exec is null) {
                scene_exec = value;
            } else {
                GameObject.Destroy(value);
                throw new SingletonOverwriteException("Scene executive already exists!");
            }
        }
    }

    /// <summary> The audio executive for managing audio playback. </summary>
    public static AudioExec Audio {
        get => audio_exec;
        set {
            if (audio_exec is null) {
                audio_exec = value;
            } else {
                GameObject.Destroy(value);
                throw new SingletonOverwriteException("Audio executive already exists!");
            }
        }
    }

    private static SceneExec scene_exec;
    private static AudioExec audio_exec;
}
