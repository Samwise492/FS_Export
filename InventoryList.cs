using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory List", menuName = "Databases/InventoryList")]
public class InventoryList : ScriptableObject
{
    public List<Item> inventoryList;
}
