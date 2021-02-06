using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(Button))]
public class DoubleCollectedCoinsRewardedAdsButton : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private GameEvent _doubleCollectedCoinsEvent = default;

    private Button _button;

    private bool _adsAlreadyShowed = false;

    private void Start()
    {
        _button = GetComponent<Button>();

        // Set interactivity to be dependent on the Placement’s status:
        _button.interactable = Advertisement.IsReady(AdsManager.Instance.rewardedVideoPlacementID);

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (_button) _button.onClick.AddListener(ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
        Advertisement.Initialize(AdsManager.Instance.playStoreGameID, AdsManager.Instance.testMode);
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo()
    {
        Advertisement.Show(AdsManager.Instance.rewardedVideoPlacementID);
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == AdsManager.Instance.rewardedVideoPlacementID && !_adsAlreadyShowed)
        {
            _button.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            _adsAlreadyShowed = true;
            _button.interactable = false;
            _doubleCollectedCoinsEvent?.Raise();
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

    private void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }

}
