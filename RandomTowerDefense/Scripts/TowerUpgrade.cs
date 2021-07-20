using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerUpgrade : MonoBehaviour
{
    public Button[] button = new Button[6];
    public TextMeshProUGUI[] text = new TextMeshProUGUI[10];
    GameObject Panel;
    PlayerCtrl playerCtrl;
    void Start()
    {
        Panel = GameObject.Find("UPgradePanel").GetComponent<Transform>().gameObject;
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        for (int i = 0; i < button.Length; i++)
        {
            button[i] = GameObject.Find("UPgradePanel").GetComponent<Transform>().GetChild(i + 1).GetComponent<Button>();
        }
        for (int i = 0; i < text.Length; i++)
        {
            text[i] = GameObject.Find("UPgradePanel").GetComponent<Transform>().GetChild(i + 12).GetComponent<TextMeshProUGUI>();
        }
        ShowText();
        SetButton();
        Panel.SetActive(false);
    }
    public void SetButton()
    {
        button[1].GetComponent<Button>().onClick.AddListener(NormalUpgrade);
        button[2].GetComponent<Button>().onClick.AddListener(MagicUpgrade);
        button[3].GetComponent<Button>().onClick.AddListener(RareUpgrade);
        button[4].GetComponent<Button>().onClick.AddListener(UniqueUpgrade);
        button[5].GetComponent<Button>().onClick.AddListener(EpicUpgrade);
    }
    public void ShowText()
    {
        playerCtrl.ShowTowerInfo();
        text[0].text = "노말 타워 LV: " + GameDB.Instance.normalTowerLV;
        text[1].text = "매직 타워 LV: " + GameDB.Instance.magicTowerLV;
        text[2].text = "레어 타워 LV: " + GameDB.Instance.rareTowerLV;
        text[3].text = "유니크 타워 LV: " + GameDB.Instance.uniqueTowerLV;
        text[4].text = "에픽 타워 LV: " + GameDB.Instance.epicTowerLV;

        if (GameDB.Instance.normalTowerLV ==30)
        {
            text[5].fontSize = 30;
            text[5].text = "최대업그레이드";
            button[1].enabled=false;
        }
        else
        {
            text[5].text = ((int)GameDB.Instance.normalUpgradeCost) + "원";
        }
        if (GameDB.Instance.magicTowerLV == 30)
        {
            text[6].fontSize = 30;
           text[6].text = "최대업그레이드";
            button[2].enabled = false;
        }
        else
        {
            text[6].text = ((int)GameDB.Instance.magicUpgradeCost) + "원";
        }
        if (GameDB.Instance.rareTowerLV == 30)
        {
            text[7].fontSize = 30;
            text[7].text = "최대업그레이드";
            button[3].enabled = false;
        }
        else
        {
            text[7].text = ((int)GameDB.Instance.rareUpgradeCost) + "원";
        }
        if (GameDB.Instance.uniqueTowerLV == 30)
        {
            text[8].fontSize = 30;
            text[8].text = "최대업그레이드";
            button[4].enabled = false;

        }
        else
        {
            text[8].text = ((int)GameDB.Instance.uniqueUpgradeCost) + "원";
        }
        if (GameDB.Instance.epicTowerLV == 30)
        {
            text[9].fontSize = 30;
            text[9].text = "최대업그레이드";
            button[5].enabled = false;
        }
        else
        {
            text[9].text = ((int)GameDB.Instance.epicUpgradeCost) + "원";
        }
       
    }
    public void OpenPanel()
    {
        Panel.SetActive(true);

    }
    public void ClosePanel()
    {
        Panel.SetActive(false);
    }

    public void NormalUpgrade()
    {
        if ((GameDB.Instance.Gold >= GameDB.Instance.normalUpgradeCost) && GameDB.Instance.normalTowerLV < 30)
        {
            GameDB.Instance.Gold -= (int)GameDB.Instance.normalUpgradeCost;
            GameDB.Instance.normalTowerLV++;
            GameDB.Instance.normalUpgradeCost += (GameDB.Instance.normalUpgradeCost * 0.15f);
            GameDB.Instance.UpgradeTowerDamage(0);
        }
        ShowText();
    }
    public void MagicUpgrade()
    {
        if ((GameDB.Instance.Gold >= GameDB.Instance.magicUpgradeCost) && GameDB.Instance.magicTowerLV < 30)
        {
            GameDB.Instance.Gold -= (int)GameDB.Instance.magicUpgradeCost;
            GameDB.Instance.magicTowerLV++;
            GameDB.Instance.magicUpgradeCost += (GameDB.Instance.magicUpgradeCost * 0.15f);
            GameDB.Instance.UpgradeTowerDamage(1);
        }
        ShowText();
    }
    public void RareUpgrade()
    {
        if ((GameDB.Instance.Gold >= GameDB.Instance.rareUpgradeCost) && GameDB.Instance.rareTowerLV < 30)
        {
            GameDB.Instance.Gold -= (int)GameDB.Instance.rareUpgradeCost;
            GameDB.Instance.rareTowerLV++;
            GameDB.Instance.rareUpgradeCost +=(GameDB.Instance.rareUpgradeCost * 0.15f);
            GameDB.Instance.UpgradeTowerDamage(2);
        }
        ShowText();
    }
    public void UniqueUpgrade()
    {
        if ((GameDB.Instance.Gold >= GameDB.Instance.uniqueUpgradeCost) && GameDB.Instance.uniqueTowerLV < 30)
        {
            GameDB.Instance.Gold -= (int)GameDB.Instance.uniqueUpgradeCost;
            GameDB.Instance.uniqueTowerLV++;
            GameDB.Instance.uniqueUpgradeCost +=(GameDB.Instance.uniqueUpgradeCost * 0.15f);
            GameDB.Instance.UpgradeTowerDamage(3);
        }
        ShowText();
    }
    public void EpicUpgrade()
    {
        if ((GameDB.Instance.Gold >= GameDB.Instance.epicUpgradeCost) && GameDB.Instance.epicTowerLV < 30)
        {
            GameDB.Instance.Gold -= (int)GameDB.Instance.epicUpgradeCost;
            GameDB.Instance.epicTowerLV++;
            GameDB.Instance.epicUpgradeCost += (GameDB.Instance.epicUpgradeCost * 0.15f);
            GameDB.Instance.UpgradeTowerDamage(4);
        }
        ShowText();
    }
}
