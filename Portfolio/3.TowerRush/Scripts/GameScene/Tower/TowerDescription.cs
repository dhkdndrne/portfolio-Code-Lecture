using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerDescription : MonoBehaviour
{
    int layerMask;
    TutorialManager tutorialManager;

    [Header("UI")]
    [SerializeField] Button infoButton;
    public SpriteRenderer towerRangeSprite;
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;


    [Header("Object")]
    [SerializeField] GameObject panel;
    [SerializeField] GameObject infoPrefab;
    bool checkTutorial;

    public void ShowTowerRange(TowerBase _Tower)
    {
        
        if (_Tower == null)
        {
            towerRangeSprite.gameObject.SetActive(false);
            nameText.text = string.Empty;
            levelText.text = string.Empty;
        }
        else
        {
            transform.GetChild(0).position = _Tower.transform.position;

            nameText.text = _Tower.towerName;
            nameText.transform.position = _Tower.transform.position + new Vector3(0, 13f, 0);
            levelText.transform.position = _Tower.transform.position + new Vector3(0, 8f, 0);

            levelText.text = "Lv : " + _Tower.level;


            towerRangeSprite.gameObject.SetActive(true);
            towerRangeSprite.transform.localScale = new Vector3(_Tower.range / 0.417f, _Tower.range / 0.417f, 1f);

        }

    }

    void Init()
    {
        TowerBase[] temp = GameManager.Instance.StageParent.transform.GetChild(DataController.CurrentStage).GetChild(5).GetComponentsInChildren<TowerBase>();
        List<TowerBase> towerList = new List<TowerBase>();
        bool check = false;

        towerList.Add(temp[0]);
        for (int i = 1; i < temp.Length; i++)
        {
            check = false;
            for (int j = 0; j < towerList.Count; j++)
            {
                if (towerList[j].towerName.Equals(temp[i].towerName))
                {
                    check = true;
                    break;
                }
            }

            if (!check) towerList.Add(temp[i]);
        }

        foreach (var tower in towerList)
        {
            GameObject infoObj = Instantiate(infoPrefab);
            infoObj.transform.SetParent(panel.transform.GetChild(0).GetChild(0).GetChild(0).transform);
            infoObj.transform.localScale = new Vector3(1, 1, 1);
            infoObj.GetComponent<TowerInfoUI>().Init(tower);
        }
    }

    private void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Tower");
        if (!UserData.Instance.userdata.isFinishTutorial)
        {
            tutorialManager = FindObjectOfType<TutorialManager>();
        }

        infoButton.onClick.AddListener(() => panel.SetActive(panel.activeSelf == false ? true : false));
        if (!UserData.Instance.userdata.isFinishTutorial) infoButton.onClick.AddListener(() => TutorialClick());

        Init();
    }

    void TutorialClick()
    {
        GameManager.Instance.tutorialManager.check = true;
        infoButton.onClick.RemoveListener(TutorialClick);
    }
    private void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0) && !GameManager.Instance.magicController.IsMagicSelected)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(pos, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                TowerBase tower = hit.transform.GetComponent<TowerBase>();
                ShowTowerRange(tower);

                //튜토리얼을 끝내지 않았을때
                if (!UserData.Instance.userdata.isFinishTutorial && !checkTutorial)
                {
                    tutorialManager.check = true;
                    checkTutorial = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            ShowTowerRange(null);
        }
    }
}
