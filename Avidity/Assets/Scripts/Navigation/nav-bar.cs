using UnityEngine;
using UnityEngine.UI;


public class NavigationBarScript : Avidity.UIElementScript
{
    void Awake()
    {
        GetComponent<Image>().color = colours.Back.Protive;
    }
}
