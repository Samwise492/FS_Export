using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class SkippableAd : MonoBehaviour
{
#if UNITY_IOS
    private string gameId = "3879250";
#elif UNITY_ANDROID
    private string gameId = "3879251";
#endif
    public string myPlacementId = "video";
    public static SkippableAd Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Advertisement.Initialize(gameId, true);
    }

    public void ShowAd()
    {
        Advertisement.Show(myPlacementId);
    }
}
