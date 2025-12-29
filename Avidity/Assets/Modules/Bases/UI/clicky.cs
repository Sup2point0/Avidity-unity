using UnityEngine;
using UnityEngine.UIElements;


namespace Avidity
{
    public static partial class Bases
    {
        public abstract class ClickyScript
        {
            public Button button;

            
            public ClickyScript(VisualElement root, string name)
            {
                this.button = root.Q<Button>(name);
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
