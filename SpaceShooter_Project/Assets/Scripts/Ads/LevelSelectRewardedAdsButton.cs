using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Button))]
public class LevelSelectRewardedAdsButton : MonoBehaviour, IUnityAdsListener
{
    private Button _button;

    private bool _adsAlreadyShowed = false;

    [SerializeField] private LevelLoader _levelLoader;

    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        if (_button == null)
        {
            _button = GetComponent<Button>();
        }

        if (_levelLoader == null)
        {
            _levelLoader = FindObjectOfType<LevelLoader>();
        }


        bool adsIsReady = Advertisement.IsReady(AdsManager.Instance.rewardedVideoPlacementID);

        // Set interactivity to be dependent on the Placement’s status:
        _button.interactable = adsIsReady;

        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        if (_animator != null)
        {
            _animator.SetBool("IsEnable", adsIsReady);
        }


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
            if (_animator != null)
            {
                _animator.SetBool("IsEnable", true);
            }
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

            if (_animator != null)
            {
                _animator.SetBool("IsEnable", false);
            }

            StartCoroutine(CloseWindowsAndLoadArcadeScene());
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

    private IEnumerator CloseWindowsAndLoadArcadeScene()
    {
        //TODO: Close open windows
        yield return new WaitForSeconds(0.5f);
        _levelLoader.LoadArcade(.7f);
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
