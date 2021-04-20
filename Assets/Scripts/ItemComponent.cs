using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemComponent : MonoBehaviour, IObjectDestroyer
{
    #region item
    private Item item; //our item
    public Item Item => item;
    #endregion
    [SerializeField] public ItemType type; // type of our item
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        item = GameManager.Instance.itemDataBase.GetItemOfID((int)type);
        spriteRenderer.sprite = item.Icon;
        GameManager.Instance.itemsContainer.Add(gameObject, this); // we give away our gameObject and his script  
    }

    public void Destroy(GameObject gameObject)
    {
        MonoBehaviour.Destroy(gameObject);    
    }
}

public enum ItemType
{
    ForcePotion = 0, 
    DamagePotion = 1, 
    HealthPotion = 2,

    Shield = 3,
    Resurrection = 4,
    ShadowBomb = 5
}
