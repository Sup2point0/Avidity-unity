using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public abstract class SelectorController : Bases.InterfaceController
{
    protected abstract void Start();

    protected void SetLabelText(Label label, string source, string default_text)
    {
        if (source is not null) {
            label.text = source;
            label.RemoveFromClassList("value-default");
            label.AddToClassList("value-set");
        }
        else {
            label.text = default_text;
            label.RemoveFromClassList("value-set");
            label.AddToClassList("value-default");
        }
    }
}
