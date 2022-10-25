using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

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
    [HideInInspector] public Shop shop;
    [SerializeField] private bool isPlayerOnScene;
    [SerializeField] private GameObject deathPanel; 

    public int levelProgress = 1;
    [SerializeField] private GameObject buttonNest;
    Color completedLevel;
    Color currentLevel;
    Color unavailableLevel;

    private void Awake()
    {       
        Instance = this;
        healthContainer = new Dictionary<GameObject, Health>();
        buffRecieverContainer = new Dictionary<GameObject, BuffReciever>();
        itemsContainer = new Dictionary<GameObject, ItemComponent>();

        isPlayerOnScene = GameObject.Find("Player") ? isPlayerOnScene = true : false;
        
    }

    private void Start()
    {
        StartCoroutine(BackToMenuDelay());
        StartCoroutine(CheckForNextLevel());
        StartCoroutine(EndOfTheGame());
        StartCoroutine(CheckForLevelProgress());

        completedLevel = new Color(0.7960784f, 0.2f, 0.04313726f, 1f);
        currentLevel = new Color(1f, 0.9529412f, 0.3803922f, 1f);
        unavailableLevel = new Color(1f, 1f, 1f, 1f);

        if (SceneManager.GetActiveScene().buildIndex == 5)
            PlayerPrefs.SetInt("progress", 4);
    }

    public void OnClickPause()
    {
        if (Time.timeScale != 0)
            Time.timeScale = 0; // speed of time flowing
        else Time.timeScale = 1;

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive); // load pause menu
    }

    public void OnClickInventory()
    {
        if (inventoryPanel.gameObject.activeSelf == false)
        {
            inventoryPanel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else if (inventoryPanel.gameObject.activeSelf == true)
        {
            inventoryPanel.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void OnClickLevels()
    {
        if (EventSystem.current.currentSelectedGameObject.name == "Level (1)")
            SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        if (EventSystem.current.currentSelectedGameObject.name == "Level (2)" && PlayerPrefs.GetInt("progress") >= 2)
            SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
        if (EventSystem.current.currentSelectedGameObject.name == "Level (3)" && PlayerPrefs.GetInt("progress") >= 3)
            SceneManager.LoadSceneAsync(4, LoadSceneMode.Single);
        if (EventSystem.current.currentSelectedGameObject.name == "Level (4)" && PlayerPrefs.GetInt("progress") == 4)
            SceneManager.LoadSceneAsync(5, LoadSceneMode.Single);
    }

    public void OnClickRestartTheLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void NullifyPlayerPrefs()
    {
        PlayerPrefs.SetInt("progress", 1);
        PlayerPrefs.SetInt("Player_Health", 100);
        PlayerPrefs.SetInt("Player_Coins", 0);
        PlayerPrefs.SetInt("ShadowBomb", 0);
        PlayerPrefs.SetInt("Resurrection", 0);
        PlayerPrefs.SetInt("Shield", 0);
        if (PlayerInventory.Instance != null)
            PlayerInventory.Instance.inventoryListObject.inventoryList.Clear();
    }

    public void OnClickMenu()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    public void OnClickPlay()
    {
        SceneManager.LoadSceneAsync(6); // load level selection
    }

    void OnDestroy()
    {
        //ReferenceHolder.Instance.OnDestroy();
    }

    private IEnumerator BackToMenuDelay()
    {      
        while (true)
        {
            if (isPlayerOnScene)
                if (Player.Instance.isDeath)
                {
                    System.Random Randomiser = new System.Random();
                    int randomNumber = Randomiser.Next(0, 3);
                    if (1 == 1)
                    {
                        yield return new WaitForSeconds(2);
                        GoogleAd.Instance.LoadRewardVideo();
                    }

                    yield return new WaitForSeconds(2);
                    deathPanel.SetActive(true);
                    yield break;                
                }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CheckForNextLevel()
    {
        yield return new WaitForEndOfFrame();
        if (LevelTeleport.Instance != null)
            if (LevelTeleport.Instance.isNextLevel)
                SkippableAd.Instance.ShowAd();
    }

    IEnumerator CheckForLevelProgress()
    {
        while (true)
        {
            if (buttonNest != null)
            {
                switch (PlayerPrefs.GetInt("progress"))
                {
                    case 0:
                        buttonNest.gameObject.transform.GetChild(0).GetComponent<Image>().color = currentLevel;
                        break;
                    case 1:
                        buttonNest.gameObject.transform.GetChild(0).GetComponent<Image>().color = currentLevel;
                        break;
                    case 2: 
                        buttonNest.gameObject.transform.GetChild(0).GetComponent<Image>().color = completedLevel;
                        buttonNest.gameObject.transform.GetChild(1).GetComponent<Image>().color = currentLevel;
                        break;
                    case 3:
                        buttonNest.gameObject.transform.GetChild(0).GetComponent<Image>().color = completedLevel;
                        buttonNest.gameObject.transform.GetChild(1).GetComponent<Image>().color = completedLevel;
                        buttonNest.gameObject.transform.GetChild(2).GetComponent<Image>().color = currentLevel;
                        break;
                    case 4:
                        buttonNest.gameObject.transform.GetChild(0).GetComponent<Image>().color = completedLevel;
                        buttonNest.gameObject.transform.GetChild(1).GetComponent<Image>().color = completedLevel;
                        buttonNest.gameObject.transform.GetChild(2).GetComponent<Image>().color = completedLevel;
                        buttonNest.gameObject.transform.GetChild(3).GetComponent<Image>().color = currentLevel;
                        break;
                }
                
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator EndOfTheGame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (Boss.Instance != null)
            {
                if (Boss.Instance.isDead)
                {
                    yield return new WaitForSeconds(4);
                    SceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);
                    yield break;
                }
            }
            
        }      
    }

}
