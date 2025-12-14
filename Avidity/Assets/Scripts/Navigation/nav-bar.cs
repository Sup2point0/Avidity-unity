using UnityEngine;
using UnityEngine.UI;


public class NavBarScript : Avidity.Bases.UIElementScript
{
    void Awake()
    {
        GetComponent<Image>().color = colours.Back.Protive;
    }
}
