using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayAds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_ANDROID
        string appKey = "1a9694ed5";
		#else
        string appKey = "unexpected_platform";
		#endif
		Debug.Log ("unity-script: LevelPlayAds Start called");

		//Dynamic config example
		IronSourceConfig.Instance.setClientSideCallbacks (true);

		string id = IronSource.Agent.getAdvertiserId ();
		Debug.Log ("unity-script: IronSource.Agent.getAdvertiserId : " + id);
		
		Debug.Log ("unity-script: IronSource.Agent.validateIntegration");
		IronSource.Agent.validateIntegration ();

		Debug.Log ("unity-script: unity version" + IronSource.unityVersion ());

        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

        //Add AdInfo Banner Events
        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;

		// SDK init
		Debug.Log ("unity-script: IronSource.Agent.init");
		IronSource.Agent.init (appKey, IronSourceAdUnits.BANNER);
    }

    private void SdkInitializationCompletedEvent(){
        Debug.Log("SDK INIT COMPLETED");
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.TOP);
    }

    /************* Banner AdInfo Delegates *************/
    //Invoked once the banner has loaded
	void OnApplicationPause (bool isPaused)
	{
		Debug.Log ("unity-script: OnApplicationPause = " + isPaused);
		IronSource.Agent.onApplicationPause (isPaused);
	}

	//Invoked once the banner has loaded
    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo) 
    {
        Debug.Log("Loaded AD:");
        Debug.Log(adInfo);
    }
    //Invoked when the banner loading process has failed.
    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError) 
    {
        Debug.Log ("unity-script: I got BannerAdLoadFailedEvent, code: " + ironSourceError.getCode () + ", description : " + ironSourceError.getDescription ());
    }
    // Invoked when end user clicks on the banner ad
    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo) 
    {
    }
    //Notifies the presentation of a full screen content following user click
    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo) 
    {
    }
    //Notifies the presented screen has been dismissed
    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo) 
    {
    }
    //Invoked when the user leaves the app
    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo) 
    {
    }
}
