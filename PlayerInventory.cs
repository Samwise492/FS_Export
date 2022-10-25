using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    #region light_piecesCount
    [SerializeField] private int light_piecesCount;
    public int Light_piecesCount
    {
        get { return light_piecesCount; }
        set
        {
            if (value >= 0)
                light_piecesCount = value;
        }
    }
    #endregion light_piecesCount
    #region items
    private List<Item> items;
    public List<Item> Items => items;
    #endregion  // items which are taking place in inventory
    [SerializeField] public Text light_piecesText;
    [SerializeField] private InventoryUIController inventoryUIController;
    public BuffReciever buffReciever;
    public static PlayerInventory Instance { get; set; } // create link on PlayerInventory script
    public InventoryList inventoryListObject;

    void Awake()
    {
        Instance = this; // it means that by Instance variable, which is described in link, we mean variable, which refers excatly in THIS PlayerInventory
    }

    private void Start()
    {
        GameManager.Instance.inventory = this;
        items = inventoryListObject.inventoryList;

        StartCoroutine(RenderLPtoString());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Add item to inventory
        if (GameManager.Instance.itemsContainer.ContainsKey(col.gameObject)) // if container is consisted of items, which have the same type as type of object which we collected
        {
            var itemComponent = GameManager.Instance.itemsContainer[col.gameObject];

            if (items.Count > inventoryUIController.cellCount) // if inventory is full
            {
                itemComponent.Destroy(col.gameObject);
            }
            else
            {
                //items.Add(itemComponent.Item);
                inventoryListObject.inventoryList.Add(itemComponent.Item);
                itemComponent.Destroy(col.gameObject);
            }  
        }
    }

    public void AddMoney()
    {
        light_piecesCount += 100;
        light_piecesText.text = light_piecesCount.ToString();
    }

    public void Resurrect()
    {
        if (Player.Instance.BonusResurrection == 0)
        {
            Debug.Log("Resurrection state is 1");
            Player.Instance.BonusResurrection = 1;
        }   
        else
        {
            Debug.Log("Resurrection state is 0");
            Player.Instance.BonusResurrection = 0;
        }
    }

    IEnumerator RenderLPtoString()
    {
        while (true)
        {
            light_piecesText.text = light_piecesCount.ToString(); // make number to become a string

            yield return new WaitForEndOfFrame();
        }
    }
}
