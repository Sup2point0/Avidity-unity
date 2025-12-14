using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


namespace Avidity
{
    public static partial class Bases
    {
        public abstract class Clicky
        {
            public Button button;

            
            public Clicky(VisualElement ui, string name)
            {
                this.button = ui.Q<Button>(name);
            }

            public abstract void BindListeners();

            public void Enable()
            {
                this.button.SetEnabled(true);
            }

            public void Disable()
            {
                this.button.SetEnabled(false);
            }
        }
    }
}
