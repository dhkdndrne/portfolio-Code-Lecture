using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPresenter : MonoBehaviour
{
    [BoxGroup("BGM")]
    [SerializeField] private TextMeshProUGUI tmpBgmValue;
    [SerializeField] private Slider bgmSlider;
    
    [BoxGroup("FX")]
    [SerializeField] private TextMeshProUGUI tmpFxValue;
    [SerializeField] private Slider fxSlider;


    [BoxGroup("팝업창 관련")]
    [SerializeField] private GameObject stageSelectPanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Button btnOpenStageSelect;

    private readonly string PLAYERPREF_BGM_KEY = "BGM";
    private readonly string PLAYERPREF_FX_KEY = "FX";
    private void Start()
    {
        PersistentAudioSettings.MusicVolume = PlayerPrefs.HasKey(PLAYERPREF_BGM_KEY) ? PlayerPrefs.GetFloat(PLAYERPREF_BGM_KEY) : 1;
        PersistentAudioSettings.MixerVolume = PlayerPrefs.HasKey(PLAYERPREF_FX_KEY) ? PlayerPrefs.GetFloat(PLAYERPREF_FX_KEY) : 1;
        
        bgmSlider.onValueChanged.AddListener(value =>
        {
            PersistentAudioSettings.MusicVolume = value;
            tmpBgmValue.text = ((int)(value * 100)).ToString();
            PlayerPrefs.SetFloat(PLAYERPREF_BGM_KEY,value);
        });
        fxSlider.onValueChanged.AddListener(value =>
        {
            PersistentAudioSettings.MixerVolume = value;
            tmpFxValue.text = ((int)(value * 100)).ToString();
            PlayerPrefs.SetFloat(PLAYERPREF_FX_KEY,value);
        });

        MasterAudio.StartPlaylist("InGame");
        bgmSlider.value = PersistentAudioSettings.MusicVolume.Value;
        fxSlider.value = PersistentAudioSettings.MixerVolume.Value;
        
        btnOpenStageSelect.onClick.AddListener(() =>
        {
            optionPanel.SetActive(false);
            stageSelectPanel.transform.parent.GetComponent<StageSelectPresenter>().Open();
        });
    }
}
