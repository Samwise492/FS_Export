using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Database", menuName = "Databases/Items")] // fileName - item which will be created by the "Item" script
                                                                                // menuName - path in game menu (RCB in Unity) by which we're able to create item
public class ItemBase : ScriptableObject // database of our existing buff items
{
    [SerializeField, HideInInspector] private List<Item> items; // list of our items
    [SerializeField] private Item currentItem;
    private int currentIndex;

    public void CreateItem() // creating of a new item
    {
        if (items == null) // if our list of item doesn't exist
            items = new List<Item>(); // create such list

        Item item = new Item(); // create a new item
        items.Add(item);
        currentItem = item;
        currentIndex = items.Count - 1; // because numbers start from 0, not from 1
    }

    public void RemoveItem()
    {
        if ((items == null) && (currentItem == null)) // if it's true, then we're about to delete unexisting dictionary or an element
            return;                                   // we won't do it, we'll just leave out of our method

        items.Remove(currentItem); // is everything is ok, and our element do exist, then we delete it

        if (items.Count > 0) // are there elements in our dictionary?
            currentItem = items[0]; // if yes, then element which we want to delete becomes to equal like zero element of the dictionary
        else CreateItem(); // create void element, which we can delete if there are no other elements in dictionary
                           // it's done in order to avoid the errors
        currentIndex = 0;
    }

    public void NextItem() // shows the next item
    {
        if (currentIndex + 1 < items.Count)
        {
            currentIndex++;
            currentItem = items[currentIndex];
        }
    }

    public void PrevItem() // shows the previous item
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            currentItem = items[currentIndex];
        }
    }

    public Item GetItemOfID(int id)
    {
        return items.Find(t => t.ID == id); // first t is sorted element. If ID of sorted elements equals to entered id, then we'll recieve this "t"-element
    }
}


[System.Serializable] // it means that we can serialize our object
public class Item
{
    #region id
    [SerializeField] private int id;
    public int ID => id;
    #endregion
    #region itemName
    [SerializeField] private string itemName;
    public string ItemName => itemName;
    #endregion
    #region description
    [SerializeField] private string description;
    public string Description => description;
    #endregion
    #region type
    [SerializeField] private BuffType type;
    public BuffType Type => type;
    #endregion
    #region value
    [SerializeField] private float value;
    public float Value => value;
    #endregion
    #region icon
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;
    #endregion

}