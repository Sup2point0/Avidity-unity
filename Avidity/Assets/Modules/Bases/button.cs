using UnityEngine;
using UnityEngine.UI;


namespace Avidity
{
    public static partial class Bases
    {
        /// <summary> Base class for a script controlling a button `GameObject`. Automatically registers button event listeners on `Start()`. </summary>
        public abstract class ButtonScript : MonoBehaviour
        {
            public Button button;


            protected virtual void Start()
            {
                button.onClick.AddListener(OnClick);
            }

            protected abstract void OnClick();
        }
    }
}
