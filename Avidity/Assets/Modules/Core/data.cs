using System.Collections.Generic;

using UnityEngine;


public static class Data
{
    public static List<Track> Tracks;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadAll()
    {
        LoadTracks();
    }

    public static void LoadTracks()
    {
        Tracks = new() {
            new Track() {
                shard = "voxel",
                name = "Voxel",
                artists = new() {
                    new Artist() {
                        shard = "sup",
                        name = "Sup#2.0",
                    },
                },
            },
            new Track() {
                shard = "synthesis",
                name = "Synthesis.",
                artists = new() {
                    new Artist() {
                        shard = "tn-shi",
                        name = "tn-shi",
                    },
                },
            },
            new Track() {
                shard = "synthesis",
                name = "Synthesis.",
                artists = new() {
                    new Artist() {
                        shard = "tn-shi",
                        name = "tn-shi",
                    },
                },
            },
            new Track() {
                shard = "synthesis",
                name = "Synthesis.",
                artists = new() {
                    new Artist() {
                        shard = "tn-shi",
                        name = "tn-shi",
                    },
                },
            },
            new Track() {
                shard = "synthesis",
                name = "Synthesis.",
                artists = new() {
                    new Artist() {
                        shard = "tn-shi",
                        name = "tn-shi",
                    },
                },
            },
            new Track() {
                shard = "synthesis",
                name = "Synthesis.",
                artists = new() {
                    new Artist() {
                        shard = "tn-shi",
                        name = "tn-shi",
                    },
                },
            },
            new Track() {
                shard = "synthesis",
                name = "Synthesis.",
                artists = new() {
                    new Artist() {
                        shard = "tn-shi",
                        name = "tn-shi",
                    },
                },
            },
            new Track() {
                shard = "synthesis",
                name = "Synthesis.",
                artists = new() {
                    new Artist() {
                        shard = "tn-shi",
                        name = "tn-shi",
                    },
                },
            },
            new Track() {
                shard = "synthesis",
                name = "Synthesis.",
                artists = new() {
                    new Artist() {
                        shard = "tn-shi",
                        name = "tn-shi",
                    },
                },
            },
            new Track() {
                shard = "synthesis",
                name = "TERABYTE CONNECTION",
                artists = new() {
                    new Artist() {
                        shard = "tn-shi",
                        name = "tn-shi",
                    },
                },
            },
        };
    }
}
