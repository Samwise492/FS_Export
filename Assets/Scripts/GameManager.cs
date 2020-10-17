using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
    #endregion
    [SerializeField] private GameObject inventoryPanel;
    public Dictionary<GameObject, Health> healthContainer; // container which contains all health attributes
    public Dictionary<GameObject, BuffReciever> buffRecieverContainer; // container which contains all received buffs
    public Dictionary<GameObject, ItemComponent> itemsContainer;
    public ItemBase itemDataBase;
    [HideInInspector] public PlayerInventory inventory;

    private void Awake()
    {
        Instance = this;
        healthContainer = new Dictionary<GameObject, Health>();
        buffRecieverContainer = new Dictionary<GameObject, BuffReciever>();
        itemsContainer = new Dictionary<GameObject, ItemComponent>();
    }

    public void OnClickPause()
    {
        if (Time.timeScale > 0)
            Time.timeScale = 0; // speed of time flowing
        else Time.timeScale = 1;
        SceneManager.LoadScene(2, LoadSceneMode.Additive); // load menu
    }

    public void OnClickInventory()
    {
        if (Time.timeScale > 0)
        {
            inventoryPanel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            inventoryPanel.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
