using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


namespace Avidity
{
    public static partial class Bases
    {
        public abstract class InterfaceController : MonoBehaviour
        {
            public VisualElement ui;


            protected void Awake()
            {
                this.ui = GetComponent<UIDocument>().rootVisualElement;
            }
        }
    }
}
