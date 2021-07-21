using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    public Text goldTxt;
    public TextMeshProUGUI roundTxt;
    public Text lifeTxt;
    public Image TowerInfoImg;
    public GameObject TowerInfoPanel;
    public GameObject optionPanel;
    public TextMeshProUGUI[] TowerInfoTxt = new TextMeshProUGUI[6];
    GameMgr gameMgr;
    public Text roundTimerTxt;
    private void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        goldTxt = GameObject.Find("Goldtxt").GetComponent<Text>();
        roundTxt = GameObject.Find("RoundTxt").GetComponent<TextMeshProUGUI>();
        lifeTxt = GameObject.Find("lifeTxt").GetComponent<Text>();
        optionPanel = GameObject.Find("Option").GetComponent<Transform>().gameObject;
        TowerInfoImg = GameObject.Find("TowerInfoPanel").GetComponent<Image>();
        TowerInfoPanel = TowerInfoImg.GetComponent<Transform>().gameObject;

        TowerInfoTxt[0] = TowerInfoImg.GetComponent<Transform>().GetChild(0).GetComponent<TextMeshProUGUI>();
        TowerInfoTxt[1] = TowerInfoImg.GetComponent<Transform>().GetChild(1).GetComponent<TextMeshProUGUI>();
        TowerInfoTxt[2] = TowerInfoImg.GetComponent<Transform>().GetChild(2).GetComponent<TextMeshProUGUI>();
        TowerInfoTxt[3] = TowerInfoImg.GetComponent<Transform>().GetChild(3).GetComponent<TextMeshProUGUI>();
        TowerInfoTxt[4] = TowerInfoImg.GetComponent<Transform>().GetChild(4).GetComponent<TextMeshProUGUI>();
        TowerInfoTxt[5] = TowerInfoImg.GetComponent<Transform>().GetChild(5).GetComponent<TextMeshProUGUI>();

        TowerInfoPanel.SetActive(false);
        optionPanel.SetActive(false);
        roundTimerTxt = GameObject.Find("RoundImg").GetComponent<Transform>().GetChild(1).GetChild(0).GetComponent<Text>();
        InvokeRepeating("ShowTxt", 0f, 0.2f);

    }

    public void ShowTxt()
    {
        if (goldTxt) goldTxt.text = GameDB.Instance.Gold.ToString();
        if (roundTxt) roundTxt.text = "Round " + GameDB.Instance.Round;
        if (lifeTxt) lifeTxt.text = GameDB.Instance.Hp.ToString();
        if (roundTimerTxt) roundTimerTxt.text = ((int)gameMgr.time).ToString();
    }
    public void On_Off_OptionPanel(int _idx)
    {
        //닫기
        if (_idx == 0)
        {
            optionPanel.SetActive(false);
        }
        //열기
        else
        {
            optionPanel.SetActive(true);
        }
    }
    public void MovePanel(int _num)
    {
        switch (_num)
        {
            //켜기
            case 0:
                TowerInfoPanel.SetActive(false);
                break;
            //끄기
            case 1:
                TowerInfoPanel.SetActive(true);
                break;
        }

    }

    public void SetTowerInfoTxt(string _name, RareList _rareList, float _attackValue, float _attackSpeed,AttackType _type,int attackCnt)
    {
        TowerInfoTxt[0].text = "이름: " + _name;
        switch (_rareList)
        {
            case RareList.NORMAL:
                TowerInfoTxt[1].text = "등급: 노말";
                break;
            //파란글씨
            case RareList.MAGIC:
                TowerInfoTxt[1].text = "등급: <color=#0000ff> 매직 </color> ";
                break;

            case RareList.RARE:
                TowerInfoTxt[1].text = "등급: <color=#5F00FF> 레어 </color> ";
                break;

            case RareList.UNIQUE:
                TowerInfoTxt[1].text = "등급: <color=#FFBB00> 유니크 </color> ";
                break;

            case RareList.EPIC:
                TowerInfoTxt[1].text = "등급: <color=#1DDB16> 에픽 </color> ";
                break;
        }

        TowerInfoTxt[2].text = "공격력: " + _attackValue;
        TowerInfoTxt[3].text = "공격 속도 : " + _attackSpeed;
        switch(_type)
        {
            case AttackType.SINGLE:
                TowerInfoTxt[4].text = "공격타입 : 단일공격";
                break;

            case AttackType.MULTI:
                TowerInfoTxt[4].text = "공격타입 : 다중공격";
                break;

            case AttackType.SPLASH:
                TowerInfoTxt[4].text = "공격타입 : 스플래쉬";
                break;

            case AttackType.MULTISPLASH:
                TowerInfoTxt[4].text = "공격타입 : 다중 스플래쉬";
                break;
        }
        TowerInfoTxt[5].text = "공격인원수 :" + attackCnt;
    }
}
