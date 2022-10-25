using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Button))]
public class AdsButton : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    private string gameId = "3879250";
#elif UNITY_ANDROID
    private string gameId = "3879251";
#endif
    public bool isForResurrection;
    [SerializeField] GameObject potionRoot;
    PlayerInventory playerInventory;
    Button myButton;
    public string myPlacementId = "rewardedVideo";

    void Start()
    {
        myButton = GetComponent<Button>();

        // Set interactivity to be dependent on the Placement’s status:
        myButton.interactable = Advertisement.IsReady(myPlacementId);

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, false);
    }

    private void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }

    // Implement a function for showing a rewarded video ad:
    public void ShowRewardedVideo()
    {
        Advertisement.Show(myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId)
        {
            myButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
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
                int randomPotionValue = Random.Range(0, 3); // 3 because upper border isn't included in random range
                ItemType itemType = (ItemType)randomPotionValue;
                itemComponent.type = itemType;
                var item = GameManager.Instance.itemDataBase.GetItemOfID((int)itemComponent.type);

                //var rewardedPotion = Instantiate(potionRoot, GameObject.Find("Player").transform);
                PlayerInventory.Instance.inventoryListObject.inventoryList.Add(item);
                InventoryUIController.Instance.RefreshShop();
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}