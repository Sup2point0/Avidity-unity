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

    private static AudioExec audio_exec;
}
