using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

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
    #endregion
    [SerializeField] public Text light_piecesText;
    public BuffReciever buffReciever;
    public static PlayerInventory Instance { get; set; } //создаём ссылку на скрипт PlayerInventory
    
    void Awake()
    {
        Instance = this; //говорим, что под переменной Instance, которую мы описали в ссылке, мы понимаем переменную, которая отсылается именно к ЭТОМУ PlayerInventory
    }

    private void Start()
    {
        GameManager.Instance.inventory = this;
        light_piecesText.text = light_piecesCount.ToString(); //make number to become a string
        items = new List<Item>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // add item to inventory
        if (GameManager.Instance.itemsContainer.ContainsKey(col.gameObject)) // if container is consisted of items, which have the same type as type of object which we collected
        {
            var itemComponent = GameManager.Instance.itemsContainer[col.gameObject];
            items.Add(itemComponent.Item);
            itemComponent.Destroy(col.gameObject);
        }
    }
}
