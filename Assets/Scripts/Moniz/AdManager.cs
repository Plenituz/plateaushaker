using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour {
	#if UNITY_ADS
	public const string GAME_ID = "1180491";
	public const string ZONE = "rewardedVideo";
	public const float MAX_TIME_TO_LOAD = 5f;

	public static AdManager instance;
    [HideInInspector]
    public bool doAds = true;

	void Awake () {
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
	#if DEBUG
			Advertisement.Initialize (GAME_ID, true);
#else
			Advertisement.Initialize (GAME_ID, false);
#endif
            DataStorer.Store(Purchaser.ADS_PATH, Purchaser.ADS_ON);
            if (DataStorer.Read(Purchaser.ADS_PATH).Equals(Purchaser.ADS_OFF))
                doAds = false;
		} else {
			Destroy (gameObject);
		}

	}

	public void ShowRewardedUnityAd(System.Action<ShowResult> callback){
        if (!doAds)
            return;

		ShowOptions options = new ShowOptions ();
		options.resultCallback = callback;
		StartCoroutine ("WaitForReadyAndShowAd", options);
	}

	IEnumerator WaitForReadyAndShowAd(ShowOptions options){
		float startTime = Time.time;
        Debug.Log("unity ad loading...");
		while (!Advertisement.IsReady ()) {
			if (Time.time - startTime > MAX_TIME_TO_LOAD) {
				//Values.MakeToast ("Could not load ad, sorry !\nTry restarting the app while having an internet connection");
				StopCoroutine ("WaitForReadyAndShowAd");
				Advertisement.Initialize (GAME_ID, true);
			}				
			yield return null;
		}
		Advertisement.Show(ZONE, options);
	}

    #region google adMob

    private InterstitialAd fullscreenAd;
    private bool requestedFullscreenAd = false;

    private BannerView bannerAd;

    public IEnumerator ShowGoogleAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-1516564916828689/2110319251";
#else 
        string adUnitId = "unexpected_platform";
#endif

        if (!doAds)
        {
            Debug.Log("no ads:" + doAds);
            yield break;
        }
        if (requestedFullscreenAd)
        {
            Debug.Log("already request");
            yield break;
        }
        requestedFullscreenAd = true;

        fullscreenAd = new InterstitialAd(adUnitId);
        fullscreenAd.OnAdClosed += OnFullScreenAdClosed;
        fullscreenAd.LoadAd(new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("FFEF7CF84A9D934ED6146FA94F3925E3").Build());

        while (!fullscreenAd.IsLoaded())
            yield return new WaitForEndOfFrame();

        UIDeathScreen ds = FindObjectOfType<UIDeathScreen>();
        while(ds == null)
        {
            yield return new WaitForSeconds(2f);
            ds = FindObjectOfType<UIDeathScreen>();
        }

        fullscreenAd.Show();
        requestedFullscreenAd = false;
    }

    void OnAdFailed(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("google ad failed to load");
    }

    void OnFullScreenAdClosed(object sender, EventArgs args)
    {
        Debug.Log("google ad closed");
        requestedFullscreenAd = false;
        fullscreenAd.Destroy();
        fullscreenAd = null;
    }
    #endregion
#endif
}
