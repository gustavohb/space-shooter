using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class AdsManager : SingletonMonoBehavior<AdsManager>, IUnityAdsListener
{
    public string rewardedVideoPlacementID = "rewardedVideo";

    public string playStoreGameID = "3433437";

    public bool testMode = true;

    public event EventHandler OnAdFinished;

    public event EventHandler OnAdFailed;

    public EventHandler OnAdsReady;

    [HideInInspector] public bool isAdsReady = false;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(playStoreGameID, testMode);
    }

    public void ShowRewaredVideo()
    {
        ShowAd(rewardedVideoPlacementID);
    }

    public void ShowAd(string placementID)
    {
        Advertisement.Show(placementID);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == rewardedVideoPlacementID)
        {
            if (showResult == ShowResult.Finished)
            {
                OnAdFinished?.Invoke(this, EventArgs.Empty);
            }
            else if (showResult == ShowResult.Failed)
            {
                // TODO
            }
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsReady(string placementId)
    {
        isAdsReady = true;
        OnAdsReady?.Invoke(this, EventArgs.Empty);
    }

    public void OnUnityAdsDidError(string message)
    {

    }

}
