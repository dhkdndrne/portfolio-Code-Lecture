using System;
using System.Collections;
using System.Collections.Generic;
using Bam.Singleton;
using UnityEngine;
using GoogleMobileAds.Api;
public class ADManager : Singleton<ADManager>
{
	public bool isTestMode = true;
    private const string FRONT_TEST_ID = "ca-app-pub-3940256099942544/8691691433";
    private const string FRONT_ID = "ca-app-pub-3252667013032639/5518016991";
    private InterstitialAd frontAD;
    
    private void Start()
    {
	    // var requestConfiguration = new RequestConfiguration.Builder()
		   //  .SetTestDeviceIds(new List<string>() { "1DF7B7CC05014E8" }) // test Device ID
		   //  .build();
	    //
	    // MobileAds.SetRequestConfiguration(requestConfiguration);
	    
	    LoadFrontAD();
	    
	    // MobileAds.Initialize((initStatus) =>
	    // {
		   //  LoadFrontAD();
	    // });
    }
    
    private void LoadFrontAD()
    {
	    frontAD = new InterstitialAd(isTestMode ? FRONT_TEST_ID : FRONT_ID);
	    frontAD.LoadAd(GetADRequest());
	    frontAD.OnAdClosed += (sender, e) => 
	    {
		    UtilClass.DebugLog("전면광고");
	    };
    }
    AdRequest GetADRequest() 
    {
	    return new AdRequest.Builder().Build();
    }
    
    public void ShowFrontAd()
    {
	    frontAD.Show();
	    LoadFrontAD();
    }
}
