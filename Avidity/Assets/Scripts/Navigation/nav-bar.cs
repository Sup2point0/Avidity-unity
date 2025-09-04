using UnityEngine;
using UnityEngine.UI;


public class NavigationBarScript : Avidity.Bases.UIElementScript
{
    void Awake()
    {
        GetComponent<Image>().color = colours.Back.Protive;
    }
}
