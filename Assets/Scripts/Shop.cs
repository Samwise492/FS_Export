using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    #region items
    private List<Item> items;
    public List<Item> Items => items;
    #endregion
    [SerializeField] private GameObject[] supplies;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.shop = this;
        items = new List<Item>();

        for (int i = 0; i < supplies.Length; i++)
        {
            var itemComponent = GameManager.Instance.itemsContainer[supplies[i]];
            items.Add(itemComponent.Item);
        }       
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject);
        /*if (GameManager.Instance.itemsContainer.ContainsKey(col.gameObject))
        {
            var itemComponent = GameManager.Instance.itemsContainer[col.gameObject];
            items.Add(itemComponent.Item);
        }*/
    }
}
