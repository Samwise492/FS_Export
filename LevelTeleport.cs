using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTeleport : MonoBehaviour
{
    public static LevelTeleport Instance { get; set; }
    [SerializeField] private int nextLevel;
    public bool isNextLevel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.levelProgress = nextLevel - 1;
        if (PlayerPrefs.GetInt("progress") < GameManager.Instance.levelProgress)
            PlayerPrefs.SetInt("progress", GameManager.Instance.levelProgress);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isNextLevel = true;
            SceneManager.LoadSceneAsync(nextLevel + 1, LoadSceneMode.Single);

            // if player goes in 2nd level (which is 3rd scene), progress equals 2, which means 2 level is now available
            if (PlayerPrefs.HasKey("progress"))
                GameManager.Instance.levelProgress = PlayerPrefs.GetInt("progress");
            isNextLevel = false;
            Player.Instance.SavePlayerPrefs();
        }          
    }
}
