using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shop : MonoBehaviour
{
    #region items
    private List<Item> items;
    public List<Item> Items => items;
    #endregion
    [SerializeField] public GameObject[] supplies;
    [SerializeField] public int[] prices;
    public Animator traderIconAnimator;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.shop = this;
        items = new List<Item>();

        for (int i = 0; i < supplies.Length; i++)
        {
            var itemComponent = GameManager.Instance.itemsContainer[supplies[i]];
            items.Add(itemComponent.Item); 
        }       
    }
}
