using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIController : InventoryUIController
{
    override public void Init()
    {
        base.Init();
    }

    override public void RefreshInventory()
    {
        var shop = GameManager.Instance.shop;
        foreach (var cell in cells)
            cell.Init(null);
        for (int i = 0; i < shop.Items.Count; i++) //i < items.Count;
        {
            if (i < cells.Length)
                cells[i].Init(shop.Items[i]); // initialize our cells
        }
    }
}
