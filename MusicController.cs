using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; set; }
    public AudioSource background;
    private AudioClip firstLvl;
    private AudioClip secondLvl;
    private AudioClip thirdLvl;
    private AudioClip fourthLvl;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            background.enabled = false;
        }
        else
        {
            if (Instance != null && Instance != this)
            {
                background.enabled = false;
            }
            else
            {
                Instance = this;
            }
        }
        StartCoroutine(CheckForMusic());
        StartCoroutine(CheckForBackgroundTrack());

        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        MobileAds.Initialize(initStatus => { });
        firstLvl = Resources.Load<AudioClip>("Sounds/Music/StartTheJourney");
        secondLvl = Resources.Load<AudioClip>("Sounds/Music/CaveNForest");
        thirdLvl = Resources.Load<AudioClip>("Sounds/Music/WaterBreeze");
        fourthLvl = Resources.Load<AudioClip>("Sounds/Music/LastFight");
    }

    IEnumerator CheckForMusic()
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name == "StartMenu")
            {
                background.enabled = false;
            }
            else
            {
                if (Instance != null && Instance != this)
                {
                    background.enabled = false;
                }
                else
                {
                    background.enabled = true;
                    if (!background.isPlaying)
                    {
                        background.Play();
                    }                   
                }
            }
            yield return new WaitForSeconds(1);
        }
        
    }

    IEnumerator CheckForBackgroundTrack()
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name == "level_1")
                background.clip = firstLvl;               
            if (SceneManager.GetActiveScene().name == "level_2")
                background.clip = secondLvl;
            if (SceneManager.GetActiveScene().name == "level_3")
                background.clip = thirdLvl;
            if (SceneManager.GetActiveScene().name == "level_4")
                background.clip = fourthLvl;
            

            yield return new WaitForSeconds(1);
        }
    }
}
