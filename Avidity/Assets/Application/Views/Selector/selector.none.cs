using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class EmptySelectorController : SelectorController
{
    public VisualElement root;


    void OnEnable()
    {
        this.root = this.ui.Q<VisualElement>("selector-none");
    }

    protected override void Start()
    {
        Exec.Scene.onSelectionChanged += OnSelectionChanged;
    }


    void OnSelectionChanged()
    {
        if (Exec.Scene.selectedEntityType == Bases.EntityType.NoSelection) {
            this.root.style.display = DisplayStyle.Flex;
        } else {
            this.root.style.display = DisplayStyle.None;
        }
    }
}
