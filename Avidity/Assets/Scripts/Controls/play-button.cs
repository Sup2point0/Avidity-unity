using UnityEngine;
using UnityEngine.UI;


public class PlayButtonScript : ButtonScript
{
    protected override void OnClick()
    {
        Exec.Audio.TogglePause();
    }
}
