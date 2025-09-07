using UnityEngine;

using Avidity;


public class ExitApplicationButtonScript : Avidity.Bases.ButtonScript
{
    protected override void OnClick()
    {
        Application.Quit();
    }
}
