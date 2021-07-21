using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
public class LobbyManager : SingleTon<LobbyManager>
{
    public delegate void OnUnitCheck();
    public OnUnitCheck onUnitCheckCallback;

    System.Action enterGame;

    int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;         
            lobbyUI.goldText.text = gold.ToString();
        }
    }

    int luga;
    public int Luga
    {
        get { return luga; }
        set
        {

#if UNITY_EDITOR

            luga = value;
            lobbyUI.diamondText.text = luga.ToString();
#elif UNITY_ANDROID

          luga = value;
          //FireBaseLogIn.Instance.UpdateUserData("luga",diamond);  
          lobbyUI.diamondText.text = luga.ToString();
        SaveData();
#endif

        }
    }

    bool isSave;
    public bool IsSave { get { return isSave; } set { isSave = value; } }
    public bool isSkipedAD; //광고 확률발동 안됐을때.
    //-------------------------------------------------------------------------------
    [HideInInspector] public int armyCount;
    [SerializeField] GameObject unitOrder;
    [SerializeField] FadeController fader;
    [SerializeField] public Text testTExt;

    [SerializeField] GameOption gameOption; //옵션관련

    /// //////////////////
    [SerializeField] PlayerMagicManager playerMagicManager;
    public TutorialManager tutorialManager;
    public FactoryController factoryManager;
    public LobbyUI lobbyUI;
    public Inventory inventory;
    public InventoryUI inventoryUI;

    private void Awake()
    {
        if (!UserData.Instance.userdata.isFinishTutorial) tutorialManager = FindObjectOfType<TutorialManager>();
        //playerMagicManager = FindObjectOfType<PlayerMagicManager>();
        //factoryManager = FindObjectOfType<FactoryController>();
        //lobbyUI = FindObjectOfType<LobbyUI>();
        //inventory = FindObjectOfType<Inventory>();

        lobbyUI.Init();
        factoryManager.Init();
        playerMagicManager.Init();
        inventory.Init();
        inventoryUI.InIt();
    }
    private void Start()
    {
        enterGame += ()=> LoadingControl.LoadScene("GameScene");

        if (!UserData.Instance.userdata.isSaved)
        {
            PlayerPrefs.DeleteAll();
        }
        else
        {
            gold = UserData.Instance.userdata.gold;
            luga = UserData.Instance.userdata.luga;
        }

        if (DataController.rewardLuga != 0 && DataController.rewardGold != 0)
        {
            gold += DataController.rewardGold;
            luga += DataController.rewardLuga;

            DataController.rewardGold = 0;
            DataController.rewardLuga = 0;
        }

        fader.gameObject.SetActive(true);

        //광고가 로딩이 됐을때, 광고제거 샀을때
        //if (!UserData.Instance.userdata.isFinishTutorial || !DataController.checkGameLoad || UserData.Instance.userdata.isBuydeleteAD || isSkipedAD)
        //{
        //    FadeOut();
        //}
        FadeOut();
        lobbyUI.diamondText.text = luga.ToString();
        lobbyUI.goldText.text = gold.ToString();
    }
    public void FadeOut()
    {
        fader.FadeOut(0.6f);
        isSkipedAD = false;
    }

    
    public void EnterGameScene()
    {
        if (armyCount > 0)
        {
            SaveGameDatacontroller();
            fader.FadeIn(0.5f, enterGame);
        }
        else
        {
            if (onUnitCheckCallback != null)
                onUnitCheckCallback.Invoke();
        }
    }

    public void SaveData()
    {
        UserData.Instance.userdata.gold = gold;
        UserData.Instance.userdata.luga = luga;
        UserData.Instance.GetInventoryItem(inventory.items);
        UserData.Instance.GetMagicInfo(playerMagicManager.MagicList);
        UserData.Instance.GetEquipedMagicInfo(playerMagicManager.equipedMagic);
        UserData.Instance.GetFactoryInfo(factoryManager.factories);

        UserData.Instance.userdata.isSaved = true;
        gameOption.SaveOptions();

#if (UNITY_ANDROID && !UNITY_EDITOR)
        FireBaseDataBase.Instance.SetData();
#endif

    }

    void SaveGameDatacontroller()
    {
        DataController.Factories = factoryManager.factories;
        DataController.PlayerMagic = playerMagicManager.equipedMagic;
        DataController.checkGameLoad = false;

        if (DataController.UnitOrder.Count > 0) DataController.UnitOrder.Clear();
        for (int i = 0; i < unitOrder.transform.childCount; i++)
        {
            DataController.UnitOrder.Add(unitOrder.transform.GetChild(i).name);
        }

#if UNITY_EDITOR

        SaveData();

#elif UNITY_ANDROID

        if (UserData.Instance.userdata.isFinishTutorial) SaveData();
        
#endif


    }
}
