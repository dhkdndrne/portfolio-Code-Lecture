using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Networking;

public class AdmobManager : MonoBehaviour
{
    public bool isTestMode;
    public Text timerText;
    public Button testBtn;
    public Text adLuga_buttonText;

    RewardedAd rewardAd;

    readonly int maxReward = 5;
    string date;
    bool isRewardFinish;

    private void Awake()
    {
        RequestRewardAD();
        RequestFrontAD();

        if (UserData.Instance.userdata.isSaved)
        {
            StartCoroutine(RewardTimer());
        }

        if (UserData.Instance.userdata.isFinishTutorial && !UserData.Instance.userdata.isBuydeleteAD && DataController.checkGameLoad)
        {
            int randomRate = UnityEngine.Random.Range(0, 100);
            if (randomRate < 20) StartCoroutine(CheckLoadedFrontAD());
            else LobbyManager.Instance.isSkipedAD = true;
        }
    }
    private void OnDestroy()
    {
        rewardAd.OnUserEarnedReward -= OnRewardAdLoaded;
        rewardAd.OnAdClosed -= HandleRewardedAdClosed;
        frontAd.OnAdClosed -= HandleRewardedAdClosed;
    }
    IEnumerator CheckLoadedFrontAD()
    {
        yield return new WaitUntil(() => rewardAd.IsLoaded());
        ShowFrontAd();
    }
    //광고보상 타이머
    IEnumerator RewardTimer()
    {
        yield return StartCoroutine(WebTimeCheck());
        DateTime playerTime = DateTimeOffset.FromUnixTimeMilliseconds(UserData.Instance.timeStamps.lugaRewardTimeStamp).DateTime.ToLocalTime();
        DateTime servertime = DateTime.Parse(date); //서버 시간을 받아온다.
        TimeSpan check = servertime - playerTime;

        if (check.Days >= 1)
        {
            testBtn.interactable = true;
            UserData.Instance.userdata.rewardCount = maxReward;
            timerText.gameObject.SetActive(false);
            adLuga_buttonText.text = "무료 " + UserData.Instance.userdata.rewardCount + "/" + maxReward;
            rewardAd.LoadAd(GetAdRequest());
        }
        else
        {
            int hour = 23 - check.Hours;
            int minute = 59 - check.Minutes;
            float second = 59 - check.Seconds;
            timerText.gameObject.SetActive(true);
            testBtn.interactable = false;
            adLuga_buttonText.text = "무료 " + UserData.Instance.userdata.rewardCount + "/" + maxReward;

            while (hour != 0 || minute != 0 || second != 0)
            {
                if (second > 0) second -= Time.deltaTime;
                else if (minute > 0)
                {
                    second = 59;
                    minute--;
                }
                else if (hour > 0)
                {
                    minute = 59;
                    hour--;
                }
                timerText.text = string.Format("{0}:{1}:{2}", hour, minute, (int)second);
                yield return null;
            }
        }
        yield return null;
    }
    public void GetDateTime()
    {
        StartCoroutine(WebTimeCheck());
    }

    //서버의 현재시간을 가져오는 코루틴
    IEnumerator WebTimeCheck()
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get("https://toweroffense-57458289-default-rtdb.firebaseio.com/"))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                date = request.GetResponseHeader("date");

                if (isRewardFinish)
                {
                    DateTime temp = DateTime.Parse(date);
                    UserData.Instance.timeStamps.lugaRewardTimeStamp = (long)((temp.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds * 1000);
                }

            }
        }
    }

    //광고 시청이 끝났을떄
    void OnRewardAdLoaded(object sender, Reward args)
    {
        LobbyManager.Instance.lobbyUI.StartResourceEff(false, 0, 20);
        UserData.Instance.userdata.rewardCount--;
        adLuga_buttonText.text = "무료 " + UserData.Instance.userdata.rewardCount + "/" + maxReward;

        if (UserData.Instance.userdata.rewardCount == 0)
        {
            testBtn.interactable = false;
            isRewardFinish = true;
            FireBaseDataBase.Instance.SetTimeStamp();
            StartCoroutine(RewardTimer());
        }
    }

    #region 리워드 광고
    const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string rewardID = "ca-app-pub-2830513154986532/9900335666";

    void RequestRewardAD()
    {
        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
        rewardAd.OnUserEarnedReward += OnRewardAdLoaded;
        rewardAd.OnAdClosed += HandleRewardedAdClosed;
        rewardAd.LoadAd(GetAdRequest());
    }
    //보상 광고 보여주기
    public void ShowRewardAD()
    {
        if (UserData.Instance.userdata.rewardCount > 0)
        {
            //광고제거 샀을때
            if (UserData.Instance.userdata.isBuydeleteAD)
            {
                OnRewardAdLoaded(null, null);
            }
            else //광고제거 안샀을때
            {
                if (rewardAd.IsLoaded())
                {
                    rewardAd.Show();
                }
                else
                {
                    RequestRewardAD();
                }
            }

        }
    }
    void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        RequestRewardAD();
    }
    #endregion

    #region 전면 광고
    const string frontTestID = "ca-app-pub-3940256099942544/1033173712";
    const string frontID = "ca-app-pub-2830513154986532/1638702262";
    InterstitialAd frontAd;

    void RequestFrontAD()
    {
        frontAd = new InterstitialAd(isTestMode ? frontTestID : frontID);
        frontAd.OnAdClosed += HandleFrontAdClosed;
        frontAd.LoadAd(GetAdRequest());
    }
    void HandleFrontAdClosed(object sender, EventArgs args)
    {
        RequestFrontAD();
    }
    public void ShowFrontAd()
    {

        if (frontAd.IsLoaded())
        {
            frontAd.Show();
        }
        else
        {
            RequestFrontAD();
        }
    }
    #endregion

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

}
