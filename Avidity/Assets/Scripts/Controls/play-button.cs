using UnityEngine;
using UnityEngine.UI;

using Avidity;


public class PlayButtonScript : ButtonScript
{
    protected override void OnClick()
    {
        Exec.Audio.TogglePause();
    }
}
