using UnityEngine;


public class RewindButtonScript : ButtonScript
{
    protected override void OnClick()
    {
        Exec.Audio.Restart();
    }
}
