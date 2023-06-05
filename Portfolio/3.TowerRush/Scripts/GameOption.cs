using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DarkTonic.MasterAudio;
public class GameOption : MonoBehaviour
{
    public bool speedItemPurchase;
    //사운드
    //-------------------------------------------
    [SerializeField] Slider bgm_slider;
    [SerializeField] Slider sfx_slider;

    [SerializeField] Sprite[] optionIcon;
    [SerializeField] Image sfx_icon;
    [SerializeField] Image bgm_icon;

    [SerializeField] Text bgm_text;
    [SerializeField] Text sfx_text;

    public Button exitButton;

    [Header("GameInfo")]
    public Button gameInfo_Button;      //확률 정보 버튼
    public Button gameInfo_ExitButton;  //확률 패널 나가기 버튼
    public GameObject gameInfo_Panel;   //확률 정보 패널
    public GameObject deleteCheckPanel; // 데이터 삭제 확인 패널
    public GameObject quitAppPanel;     //앱 종료 안내패널

    public Button quitAppBtn;           // 앱 나가기 확인 버튼
    public Button deleteData_Button;    // 파이어베이스 정보 삭제 버튼
    public Button undodelete_Button;    // 취소버튼
    public Button openPanel_Button;     // 데이터 삭제 패널 열기버튼

    public Button savedata_Button;      // 저장버튼

    public float sfx_value;
    public float sfx_temp;

    public float bgm_value;
    public float bgm_temp;

    public bool isBgm_mute;
    public bool isSfx_mute;

    private void Start()
    {
        float bgm = PlayerPrefs.HasKey("Bgm_Value") == true ? PlayerPrefs.GetFloat("Bgm_Value") : 1;
        float sfx = PlayerPrefs.HasKey("Sfx_Value") == true ? PlayerPrefs.GetFloat("Sfx_Value") : 1;

        float tempBgm = PlayerPrefs.HasKey("Bgm_tempValue") == true ? PlayerPrefs.GetFloat("Bgm_tempValue") : 0;
        float tempSfx = PlayerPrefs.HasKey("Sfx_tempValue") == true ? PlayerPrefs.GetFloat("Sfx_tempValue") : 0;

        bool bgmMute = PlayerPrefs.HasKey("Bgm_Mute") == true ? System.Convert.ToBoolean(PlayerPrefs.GetInt("Bgm_Mute")) : false;
        bool sfxMute = PlayerPrefs.HasKey("Sfx_Mute") == true ? System.Convert.ToBoolean(PlayerPrefs.GetInt("Sfx_Mute")) : false;
        SetValues(bgm, tempBgm, bgmMute, sfx, tempSfx, sfxMute);

        gameInfo_Button?.onClick.AddListener(() => gameInfo_Panel.SetActive(true));
        gameInfo_ExitButton?.onClick.AddListener(() => gameInfo_Panel.SetActive(false));
        quitAppBtn?.onClick.AddListener(() =>Application.Quit());
       
        if (SceneManager.GetActiveScene().name.Equals("MainScene"))
        {
            openPanel_Button.onClick.AddListener(() =>
            {
                StartCoroutine(LobbyManager.Instance.lobbyUI.PanelAnim(deleteCheckPanel, false));
            });

            deleteData_Button.onClick.AddListener(() =>
            {
                FireBaseDataBase.Instance.OnClickRemove();
                LobbyManager.Instance.lobbyUI.optionPanel.SetActive(false);
                StartCoroutine(LobbyManager.Instance.lobbyUI.PanelAnim(deleteCheckPanel, true));
                StartCoroutine(LobbyManager.Instance.lobbyUI.PanelAnim(quitAppPanel, false));
            });

            undodelete_Button.onClick.AddListener(() =>
            {
                StartCoroutine(LobbyManager.Instance.lobbyUI.PanelAnim(deleteCheckPanel, true));
            });
           

            savedata_Button.onClick.AddListener(LobbyManager.Instance.SaveData);
        }

        if (SceneManager.GetActiveScene().name.Equals("GameScene")) SetWorldBGM();
    }

    void SetWorldBGM()
    {
        if (DataController.CurrentStage / 10 == 0)
        {
            MasterAudio.TriggerPlaylistClip("World_1");
        }
        else if (DataController.CurrentStage / 10 == 1)
        {
            MasterAudio.TriggerPlaylistClip("World_2");
        }
        else if (DataController.CurrentStage / 10 == 2)
        {
            MasterAudio.TriggerPlaylistClip("World_3");
        }
        else if (DataController.CurrentStage / 10 == 3)
        {
            MasterAudio.TriggerPlaylistClip("World_4");
        }
        else if (DataController.CurrentStage / 10 == 4)
        {
            MasterAudio.TriggerPlaylistClip("World_5");
        }                         
    }
    public void SetObjActiveFalse(GameObject _Object)
    {
        _Object.SetActive(false);
    }
    private void Update()
    {
        ChangeIcon();
    }
    public void SetValues(float _Bgm, float _TempBgm, bool _BgmMute, float _Sfx, float _TempSfx, bool _SfxMute)
    {
        isBgm_mute = _BgmMute;
        isSfx_mute = _SfxMute;

        bgm_slider.value = isBgm_mute == true ? 0 : _Bgm;
        sfx_slider.value = isSfx_mute == true ? 0 : _Sfx;

        bgm_temp = _TempBgm;
        sfx_temp = _TempSfx;

        sfxSliderChange();
        bgmSliderChange();
    }

    public void Mute(int _Num)
    {
        // 배경음
        if (_Num.Equals(0))
        {
            isBgm_mute = isBgm_mute == false ? true : false;

            if (isBgm_mute)
            {
                bgm_temp = bgm_slider.value;
                bgm_slider.value = 0;
            }
            else
            {
                bgm_slider.value = bgm_temp;
            }

            if (bgm_slider.value == 1f) MasterAudio.PlaylistMasterVolume = 0.99f;
            else MasterAudio.PlaylistMasterVolume = bgm_slider.value;

        }
        else //사운드
        {
            isSfx_mute = isSfx_mute == false ? true : false;
            if (isSfx_mute)
            {
                sfx_temp = sfx_slider.value;
                sfx_slider.value = 0;
            }
            else
            {
                sfx_slider.value = sfx_temp;
            }
            if (sfx_slider.value == 1f) MasterAudio.MasterVolumeLevel = 0.99f;
            else MasterAudio.MasterVolumeLevel = sfx_slider.value;
        }

    }

    void ChangeIcon()
    {
        if (sfx_value.Equals(0) || isSfx_mute) sfx_icon.sprite = optionIcon[0];
        else if (sfx_value > 0 && sfx_value < 0.3f) sfx_icon.sprite = optionIcon[1];
        else if (sfx_value >= 0.3f && sfx_value < 0.6f) sfx_icon.sprite = optionIcon[2];
        else if (sfx_value >= 0.6f && sfx_value <= 1) sfx_icon.sprite = optionIcon[3];


        if (bgm_value.Equals(0) || isBgm_mute) bgm_icon.sprite = optionIcon[0];
        else if (bgm_value > 0 && bgm_value < 0.3f) bgm_icon.sprite = optionIcon[1];
        else if (bgm_value >= 0.3f && bgm_value < 0.6f) bgm_icon.sprite = optionIcon[2];
        else if (bgm_value >= 0.6f && bgm_value <= 1) bgm_icon.sprite = optionIcon[3];

    }

    public void sfxSliderChange()
    {
        MasterAudio.MasterVolumeLevel = sfx_slider.value;
        sfx_value = sfx_slider.value;


        if (sfx_value.Equals(0)) isSfx_mute = true;
        else isSfx_mute = false;

        if (sfx_value == 1 || sfx_value == 0.99f) sfx_text.text = "100";
        else sfx_text.text = ((int)(sfx_value * 100f)).ToString();
    }
    public void bgmSliderChange()
    {
        MasterAudio.PlaylistMasterVolume = bgm_slider.value;
        bgm_value = bgm_slider.value;

        if (bgm_value.Equals(0)) isBgm_mute = true;
        else isBgm_mute = false;

        if (bgm_value == 1 || bgm_value == 0.99f) bgm_text.text = "100";
        else bgm_text.text = ((int)(bgm_value * 100f)).ToString();
     
    }

    public void OpenUrl()
    {
        Application.OpenURL("https://docs.google.com/spreadsheets/d/14eFmf6NgY1PZYfqx6PaPxuob5kBEs-pIkmtxJGjk7HU/edit?usp=sharing");
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetInt("PurChase_SpeedItem", System.Convert.ToInt32(speedItemPurchase));

        PlayerPrefs.SetFloat("Bgm_Value", bgm_value);
        PlayerPrefs.SetFloat("Sfx_Value", sfx_value);

        PlayerPrefs.SetFloat("Bgm_tempValue", bgm_temp);
        PlayerPrefs.SetFloat("Sfx_tempValue", sfx_temp);

        PlayerPrefs.SetInt("Bgm_Mute", System.Convert.ToInt32(isBgm_mute));
        PlayerPrefs.SetInt("Sfx_Mute", System.Convert.ToInt32(isSfx_mute));
        PlayerPrefs.Save();
    }
}
