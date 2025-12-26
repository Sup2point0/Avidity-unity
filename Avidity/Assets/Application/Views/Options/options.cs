using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class OptionsWindowController : Bases.InterfaceController
{
    public SaveClicky saveClicky;


    void OnEnable()
    {
        this.saveClicky = new SaveClicky(this.ui);
    }

    void Start()
    {
        this.saveClicky.BindListeners();
    }
}


public class SaveClicky : Bases.Clicky
{
    public SaveClicky(VisualElement ui) : base(ui, "save") {}

    public override void BindListeners()
    {
        this.button.clicked += () => Persistence.SaveOptions();
    }
}
