using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class FireBaseLogIn : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseUser user;
    FirebaseApp firebaseApp;
    DatabaseReference reference;

    [SerializeField] TitleUI titleUI;
    //기기 연동이 되어있는 상태인지 체크
    bool signedIn;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        firebaseApp = FirebaseDatabase.DefaultInstance.App;
        //데이터베이스 URl
        firebaseApp.Options.DatabaseUrl = new Uri("https://toweroffense-57458289-default-rtdb.firebaseio.com/");
        //DatabaseReference에 접근
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Start()
    {
#if UNITY_EDITOR
        titleUI.text.text = "에디터 로그인";
        int level = 30;
        int gold = 0;
        int luga = 0;
        int stage = 2;
        int exp = 0;
        UserData.Instance.userdata = new UserInfo(Social.localUser.id, level, exp, gold, luga, stage,5, false, true, false);
        SceneManager.LoadSceneAsync(4);
#elif UNITY_ANDROID
        CheckVersion();
#endif
    }

    public void LogIn()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder()
   .RequestIdToken()
   .RequestEmail()
   .Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;

        titleUI.text.text = "구글 로그인 시도중...";
        TryGoogleLogin();

    }

    public void CheckVersion()
    {
        reference.Child("version").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                //서버의 앱버전과 애플리케이션의 버전이 같을때 로그인
                if (Application.version.Equals(snapshot.Value.ToString()))
                {
                    LogIn();
                }
                else
                {
                    titleUI.panel.SetActive(true);
                }
            }
        });
    }

    private void LoadUserData()
    {
        reference.Child("users").Child(user.UserId).GetValueAsync().ContinueWith(task =>
        {
            // 저장된 정보가 없을때
            if (task.Result.Value == null)
            {
                titleUI.text.text = "신규유저 정보 생성";
                SetNewUserData();
            }
            else if (task.IsCompleted)  //저장된 정보가 있을때
            {
                titleUI.text.text = "정보 불러오는 중...";
                DataSnapshot snapshot = task.Result;
                LoadUserData(snapshot);

            }
            else if (task.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {task.Exception}");
            }
        });
    }
    void LoadUserData(DataSnapshot snapshot)
    {
        //---------------------------------------------------------------------   
        try
        {
            string id = snapshot.Child("userdata").Child("uID").Value.ToString();
            int level = (int)Convert.ChangeType(snapshot.Child("userdata").Child("level").Value, typeof(int));
            int exp = (int)Convert.ChangeType(snapshot.Child("userdata").Child("exp").Value, typeof(int));
            int gold = (int)Convert.ChangeType(snapshot.Child("userdata").Child("gold").Value, typeof(int));
            int luga = (int)Convert.ChangeType(snapshot.Child("userdata").Child("luga").Value, typeof(int));
            int stage = (int)Convert.ChangeType(snapshot.Child("userdata").Child("stage").Value, typeof(int));
            int rewardCount = (int)Convert.ChangeType(snapshot.Child("userdata").Child("rewardCount").Value, typeof(int));
            bool isSaved = Convert.ToBoolean(snapshot.Child("userdata").Child("isSaved").Value);
            bool isFinishTutorial = Convert.ToBoolean(snapshot.Child("userdata").Child("isFinishTutorial").Value);
            bool isBuydeleteAD = Convert.ToBoolean(snapshot.Child("userdata").Child("isBuydeleteAD").Value);
            UserData.Instance.userdata = new UserInfo(id, level, exp, gold, luga, stage, rewardCount, isSaved, isFinishTutorial, isBuydeleteAD);

            long lugaRewardTime = (long)Convert.ChangeType(snapshot.Child("timeStamps").Child("lugaRewardTimeStamp").Value, typeof(long));
            UserData.Instance.timeStamps = new TimeStamps(0, lugaRewardTime);
            //인벤토리
            foreach (var invenItem in snapshot.Child("inventoryItem").Children)
            {
                string invenId = invenItem.Child("id").Value.ToString();
                Item.ItemRank rank = (Item.ItemRank)Enum.Parse(typeof(Item.ItemRank), Convert.ToString(invenItem.Child("rank").Value));
                Item.ItemType type = (Item.ItemType)Enum.Parse(typeof(Item.ItemType), Convert.ToString(invenItem.Child("type").Value));

                UserData.Instance.inventoryItem.Add(new ItemInfo(invenId, rank, type));
            }

            //장착 마법
            foreach (var magic in snapshot.Child("equipedMagicList").Children)
            {
                string magicname = (string)Convert.ChangeType(magic.Child("name").Value, typeof(string));
                int magiclevel = (int)Convert.ChangeType(magic.Child("level").Value, typeof(int));
                int possessionCount = (int)Convert.ChangeType(magic.Child("possessionCount").Value, typeof(int));
                int levelUp_Price = (int)Convert.ChangeType(magic.Child("levelUp_Price").Value, typeof(int));
                bool isEquip = Convert.ToBoolean(magic.Child("isEquip").Value);

                UserData.Instance.equipedMagicList.Add(new MagicInfo(magicname, magiclevel, possessionCount, levelUp_Price, isEquip));
            }

            //마법
            foreach (var magic in snapshot.Child("magicList").Children)
            {
                string magicname = (string)Convert.ChangeType(magic.Child("name").Value, typeof(string));
                int magiclevel = (int)Convert.ChangeType(magic.Child("level").Value, typeof(int));
                int possessionCount = (int)Convert.ChangeType(magic.Child("possessionCount").Value, typeof(int));
                int levelUp_Price = (int)Convert.ChangeType(magic.Child("levelUp_Price").Value, typeof(int));
                bool isEquip = Convert.ToBoolean(magic.Child("isEquip").Value);

                UserData.Instance.magicList.Add(new MagicInfo(magicname, magiclevel, possessionCount, levelUp_Price, isEquip));
            }
            //팩토리
            foreach (var factory in snapshot.Child("factoryInfo").Children)
            {
                ItemInfo head = new ItemInfo(
                    factory.Child("head").Child("id").Value.ToString(),
                    (Item.ItemRank)Enum.Parse(typeof(Item.ItemRank), Convert.ToString(factory.Child("head").Child("rank").Value)),
                    (Item.ItemType)Enum.Parse(typeof(Item.ItemType), Convert.ToString(factory.Child("head").Child("type").Value)));

                ItemInfo scroll = new ItemInfo(
                    factory.Child("scroll").Child("id").Value.ToString(),
                    (Item.ItemRank)Enum.Parse(typeof(Item.ItemRank), Convert.ToString(factory.Child("scroll").Child("rank").Value)),
                    (Item.ItemType)Enum.Parse(typeof(Item.ItemType), Convert.ToString(factory.Child("scroll").Child("type").Value)));

                ItemInfo armor = new ItemInfo(
                    factory.Child("armor").Child("id").Value.ToString(),
                    (Item.ItemRank)Enum.Parse(typeof(Item.ItemRank), Convert.ToString(factory.Child("armor").Child("rank").Value)),
                    (Item.ItemType)Enum.Parse(typeof(Item.ItemType), Convert.ToString(factory.Child("armor").Child("type").Value)));

                ItemInfo shoe = new ItemInfo(
                    factory.Child("shoe").Child("id").Value.ToString(),
                    (Item.ItemRank)Enum.Parse(typeof(Item.ItemRank), Convert.ToString(factory.Child("shoe").Child("rank").Value)),
                    (Item.ItemType)Enum.Parse(typeof(Item.ItemType), Convert.ToString(factory.Child("shoe").Child("type").Value)));

                bool isPresent = Convert.ToBoolean(factory.Child("isPresent").Value);

                UserData.Instance.factoryInfo.Add(new FactoryInfo(head, scroll, armor, shoe, isPresent));
            }
            //유저가 클리어한 스테이지까지 첫 보상을 비활성화 시킨다.
            DBManager.Instance.worldDB.SetClearWorldInfo(stage);
        }
        catch (Exception e)
        {
            titleUI.text.text = e.ToString();
        }

        FireBaseDataBase.Instance.auth = auth;
        FireBaseDataBase.Instance.user = user;
        FireBaseDataBase.Instance.firebaseApp = firebaseApp;
        FireBaseDataBase.Instance.reference = reference;

        SceneManager.LoadSceneAsync(1);
    }

    //구글 로그인
    public void TryGoogleLogin()
    {
        Social.localUser.Authenticate((bool success) => // 로그인 시도
        {
            if (success) // 성공하면
            {
                TryFirebaseLogin(); // Firebase Login 시도
            }
            else // 실패하면
            {
                titleUI.text.text = "구글 로그인 실패";
            }
        });
    }
    //구글 로그아웃
    public void TryGoogleLogout()
    {
        if (Social.localUser.authenticated) // 로그인 되어 있다면
        {
            PlayGamesPlatform.Instance.SignOut(); // Google 로그아웃
            auth.SignOut(); // Firebase 로그아웃
        }
    }
    //파이어베이스 로그인
    void TryFirebaseLogin()
    {
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                titleUI.text.text = "파이어베이스 실패!";
                return;
            }
            if (task.IsFaulted)
            {
                titleUI.text.text = "파이어베이스 실패2";
                return;
            }

            //연동되었으면
            if (signedIn)
            {
                titleUI.text.text = "저장된 정보 확인중...";
                LoadUserData();
            }
        });
    }

    //계정 로그인에 어떠한 변경점이 발생시 진입.
    void AuthStateChanged(object _Sender, System.EventArgs _EventArgs)
    {
        if (auth.CurrentUser != user)
        {
            //연동된 계정과 기기의 계정이 같다면 true 리턴
            signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            user = auth.CurrentUser;
        }
    }


    //신규 유저 데이터 입력
    void SetNewUserData()
    {
        FireBaseDataBase.Instance.auth = auth;
        FireBaseDataBase.Instance.user = user;
        FireBaseDataBase.Instance.firebaseApp = firebaseApp;
        FireBaseDataBase.Instance.reference = reference;

        string id = Social.localUser.userName;
        int gold = 0;
        int luga = 40;
        int level = 1;
        int stage = 0;
        int exp = 0;

        UserData.Instance.userdata = new UserInfo(id, level, exp, gold, luga, stage, 5);
        SceneManager.LoadSceneAsync(4);
    }
}
