using UnityEngine;


public class SkipButtonScript : ButtonScript
{
    protected override void OnClick()
    {
        Exec.Audio.PlayNext();
    }
}
