using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class ConfigWindowController : Bases.InterfaceController
{
    public SaveOptionsClicky saveOptionsClicky;
    public SaveDataClicky saveDataClicky;


    void OnEnable()
    {
        this.saveOptionsClicky = new SaveOptionsClicky(this.ui);
        this.saveDataClicky    = new SaveDataClicky(this.ui);
    }

    void Start()
    {
        this.saveOptionsClicky.BindListeners();
        this.saveDataClicky.BindListeners();
    }
}


public class SaveOptionsClicky : Bases.ClickyScript
{
    public SaveOptionsClicky(VisualElement ui) : base(ui, "save-options") {}

    public override void BindListeners()
    {
        this.button.clicked += () => Persistence.SaveOptions();
    }
}


public class SaveDataClicky : Bases.ClickyScript
{
    public SaveDataClicky(VisualElement ui) : base(ui, "save-data") {}

    public override void BindListeners()
    {
        this.button.clicked += () => Persistence.SaveData();
    }
}
