using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


[UxmlElement]
public partial class GridRow<Cell, Entity> : VisualElement
    where Cell : VisualElement, Bases.IBindableItem<Entity>, new()
{
    public int cellsPerRow;


    public GridRow() {}

    public GridRow(VisualTreeAsset cell_uxml)
    {
        this.style.flexDirection  = FlexDirection.Row;
        this.style.justifyContent = Justify.SpaceAround;

        for (int i = 0; i< this.cellsPerRow; i++) {
            var cell = new Cell();
            cell.InitFromUxml(cell_uxml);
            this.Add(cell);
        }
    }

    public void BindItems(Entity[] entities)
    {
        this.Children().Zip(entities, (cell, e) => {
            (cell as Cell).Bind(e);
            return 1;
        }).ToList();
    }

    public void UnbindItems()
    {
        foreach (var cell in this.Children()) {
            (cell as Cell).Unbind();
        }
    }
}
