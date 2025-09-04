using UnityEngine;

using Avidity;


public class RewindButtonScript : ButtonScript
{
    protected override void OnClick()
    {
        Exec.Audio.Restart();
    }
}
