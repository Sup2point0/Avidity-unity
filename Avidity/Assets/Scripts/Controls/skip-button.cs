using UnityEngine;

using Avidity;


public class SkipButtonScript : ButtonScript
{
    protected override void OnClick()
    {
        Exec.Audio.PlayNext();
    }
}
