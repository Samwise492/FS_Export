using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : InventoryUIController
{
    override public void Init()
    {
        base.Init();
    }

    override public void RefreshShop()
    {
        var shop = GameManager.Instance.shop;
        foreach (var cell in cells)
            cell.Init(null);
        for (int i = 0; i < shop.Items.Count; i++) //i < items.Count;
        {            
            if (i < cells.Length)
            {
                cells[i].Init(shop.Items[i]); // initialize our cells

                var price = cells[i].GetComponentInChildren<Text>(); // price of one cell in the shop
                var icon = cells[i].transform.GetChild(0).GetComponent<Image>();//.GetComponentInChildren<Image>();
                price.text = shop.prices[i].ToString();
                icon.sprite = shop.sprites[i];
            }
            
        }
    }
}
