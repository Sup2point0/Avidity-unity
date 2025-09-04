using UnityEngine;
using UnityEngine.UI;

using Avidity;


public class ControlsScript : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Image>().color = Colours.Back.PROT;
    }
}
