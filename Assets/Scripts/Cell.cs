using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Cell : MonoBehaviour // cell of inventory
{
    [SerializeField] private Image icon;
    private Item item;
    public Action OnRefreshCell;

    private void Awake()
    {
        icon.sprite = null; // wash the icon, in order to one icon wouldn't be over other one
    }

    public void Init(Item item)
    {
        this.item = item; //initialize our item
        if (item == null)
            icon.sprite = null;
        else
            icon.sprite = item.Icon; //change sprite of the icon on sprite of the item
    }

    public void OnClickCell()
    {
        if (item == null)
            return;
        GameManager.Instance.inventory.Items.Remove(item); // delete item from the list
        Buff buff = new Buff
        {
            type = item.Type, // initialize the type of buff
            bonus = item.Value // what we add to abilities
        };
        GameManager.Instance.inventory.buffReciever.AddBuff(buff);
        if (OnRefreshCell != null)
            OnRefreshCell();
    }
}
