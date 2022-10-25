using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;

public class GoogleAd : MonoBehaviour
{
    public static GoogleAd Instance { get; set; }
    RewardedAd rewardedAd;
#if UNITY_EDITOR
    string adUnitId = "unused";
#elif UNITY_ANDROID
string adUnitId =  "ca-app-pub-1987716436705977/5650071527";
#endif
    public bool isDeath;
    public bool isForResurrection;
    [SerializeField] GameObject potionRoot;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.rewardedAd = new RewardedAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);

        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    }

    public void LoadRewardVideo()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        if (isDeath)
        {
            return;
        }
        // Get resurrection point
        if (isForResurrection)
        {
            if (gameObject.transform.parent.transform.parent.GetComponent<PanelPopUp>().additionalWindow != null)
            {
                Player.Instance.BonusResurrection = 1;
                gameObject.transform.parent.transform.parent.GetComponent<PanelPopUp>().ShowAdditionalWindow();
            }
        }
        // Get random potion
        else
        {
            ItemComponent itemComponent = potionRoot.GetComponentInChildren(typeof(ItemComponent)) as ItemComponent;
            int randomPotionValue = UnityEngine.Random.Range(0, 3); // 3 because upper border isn't included in random range
            ItemType itemType = (ItemType)randomPotionValue;
            itemComponent.type = itemType;
            var item = GameManager.Instance.itemDataBase.GetItemOfID((int)itemComponent.type);

            //var rewardedPotion = Instantiate(potionRoot, GameObject.Find("Player").transform);
            PlayerInventory.Instance.inventoryListObject.inventoryList.Add(item);
            InventoryUIController.Instance.RefreshShop();
        }
    }

}
