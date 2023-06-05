using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LoginManager : MonoBehaviour
{
	#region Inspector

	[SerializeField] private TitlePresenter titlePresenter;

    #endregion

	#region Field

	private readonly string URL = "https://samurai-slash-60ac1-default-rtdb.firebaseio.com/";

	private DatabaseReference reference;
	private FirebaseAuth auth;
	private FirebaseUser user;
	private bool isSigned;

    #endregion

	private void Awake()
	{
		Application.targetFrameRate = 60;
		titlePresenter.Init();

		FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri(URL);
		reference = FirebaseDatabase.DefaultInstance.RootReference;

#if UNITY_EDITOR

		UserData userData = new UserData("TestNick", "EditorTest", 1, false);
		DataManager.Instance.InitUserData(userData);
		GameManager.Instance.GameModel.maxStage = DataManager.Instance.MapSaveDataDic.Count;
		//GameManager.Instance.GameModel.maxStage = 1;
#elif UNITY_ANDROID
		auth = FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;

		CheckVersion();
#endif
	}

	private void Login()
	{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
				.Builder()
			.RequestServerAuthCode(false)
			.RequestIdToken()
			.Build();

		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();

		TryGoogleLogin();
	}

	private void TryGoogleLogin()
	{
		PlayGamesPlatform.Instance.Authenticate((bool success) =>
		{
			if (success)
			{
				TryFirebaseLogin();
			}
			else
			{
				Debug.Log("구글 로그인 실패");
				titlePresenter.FailedLogin();
			}
		});
	}
	private void CheckVersion()
	{
		reference.Child("version").GetValueAsync().ContinueWith(task =>
		{
			if (task.IsCompleted)
			{
				DataSnapshot snapshot = task.Result;

				//서버의 앱버전과 애플리케이션의 버전이 같을때 로그인
				if (Application.version.Equals(snapshot.Value))
				{
					Login();
				}
				else
				{
					titlePresenter.OpenVersionPanel();
				}
			}
		});
	}
	private void TryFirebaseLogin()
	{
		//string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
		string idToken = PlayGamesPlatform.Instance.GetServerAuthCode();

		Credential credential = PlayGamesAuthProvider.GetCredential(idToken);
		auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
		{
			if (task.IsCanceled)
			{
				Debug.Log("파베 로그인 실패 isCanceled");
				titlePresenter.FailedLogin();
				return;
			}

			if (task.IsFaulted)
			{
				Debug.Log("파베 로그인 실패 isFaulted");
				titlePresenter.FailedLogin();
				return;
			}
			//연동되었으면
			if (isSigned)
			{
				LoadData();
			}
		});
	}

	private void AuthStateChanged(object _Sender, System.EventArgs _EventArgs)
	{
		if (auth.CurrentUser != user)
		{
			Debug.Log("계정 변경");
			//연동된 계정과 기기의 계정이 같다면 true 리턴
			isSigned = user != auth.CurrentUser && auth.CurrentUser != null;
			user = auth.CurrentUser;
		}
	}

	private void LoadData()
	{
		reference.Child("users").Child(user.UserId).GetValueAsync().ContinueWith(task =>
		{
			Debug.Log("1: " + user.UserId);
			Debug.Log("2 :" + auth.CurrentUser.UserId);

			// 저장된 정보가 없을때
			if (task.Result.Value == null)
			{
				Debug.Log("신규 정보 생성");
				//titleUI.text.text = "신규유저 정보 생성";
				CreateNewUserData();

			}
			else if (task.IsCompleted) //저장된 정보가 있을때
			{
				Debug.Log("유저 정보 로드!");
				//titleUI.text.text = "정보 불러오는 중...";
				DataSnapshot snapshot = task.Result;
				LoadUserData(snapshot);
			}
			else if (task.Exception != null)
			{
				Debug.LogWarning(message: $"Failed to register task with {task.Exception}");
			}
			titlePresenter.ReadyToStart();
		});
	}

	private void CreateNewUserData()
	{
		Debug.Log("신규유저 정보 생성");

		GameManager.Instance.GameModel.maxStage = 1;
		UserData userData = new UserData(user.DisplayName, user.UserId, 1, false);
		DataManager.Instance.InitUserData(userData);
		DataManager.Instance.WriteData();
	}

	private void LoadUserData(DataSnapshot snapshot)
	{
		var dic = (IDictionary<string, object>)snapshot.Value;

		string nickName = dic["nickName"].ToString();
		string id = dic["uid"].ToString();
		int maxLevel = Convert.ToInt32(dic["maxStage"]);
		bool isBuyAD = Convert.ToBoolean(dic["isBuyAD"]);

		UserData userData = new UserData(nickName, id, maxLevel, isBuyAD);
		DataManager.Instance.InitUserData(userData);

		GameManager.Instance.GameModel.maxStage = maxLevel;
		IAPController.Init(isBuyAD);
	}
}
