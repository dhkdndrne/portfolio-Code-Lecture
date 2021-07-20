using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MissionMgr : MonoBehaviour
{
    public static MissionMgr instance;
    public bool[] h_Quest = new bool[10];
    public GameObject[] clearImg;
    //미션볼수있는 패널
    public GameObject hiddenMissionPanel;

    //미션 성공하면 알림창용
    public GameObject[] questSuccessPanel;
    public TextMeshProUGUI[] text;
    public Button exitBtn;
    public Button openBtn;
    Vector3 target;// = new Vector3(28f, 180f, 0f);
    //이어하기인지
    bool isContinue;
    //한번만 실행되게하는용
    bool isFirst = true;


    void Start()
    {
        clearImg = new GameObject[10];
        questSuccessPanel = new GameObject[10];
        text = new TextMeshProUGUI[10];
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene" && isFirst)
        {
            target = new Vector3(Camera.main.pixelWidth / 2.0f, Camera.main.pixelHeight / 1.2f);
            //ui설정
            hiddenMissionPanel = GameObject.Find("MissionPanel").GetComponent<Transform>().gameObject;

            for (int i = 0; i < questSuccessPanel.Length; i++)
            {
                questSuccessPanel[i] = GameObject.Find("QuestSuccessPanel").GetComponent<Transform>().GetChild(i).GetComponent<Transform>().gameObject;
            }

            for (int i = 0; i < text.Length; i++)
            {
                text[i] = questSuccessPanel[i].GetComponent<Transform>().GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            }

            exitBtn = hiddenMissionPanel.GetComponent<Transform>().GetChild(1).GetComponent<Button>();
            exitBtn.onClick.AddListener(ExitPanel);
            //버튼이벤트 설정
            openBtn = GameObject.Find("HiddenMissionBtn").GetComponent<Button>();
            openBtn.onClick.AddListener(OpenPanel);

            for (int i = 0; i < clearImg.Length; i++)
            {
                clearImg[i] = hiddenMissionPanel.GetComponent<Transform>().GetChild(0).GetChild(0).GetChild(i).GetChild(2).GetComponent<Transform>().gameObject;
                //이어하기이면 이미지 설정
                if (isContinue)
                {
                    if (h_Quest[i])
                    {
                        //퀘스트 클리어 이미지 활성화
                        clearImg[i].SetActive(true);
                        //이미지 비활성화
                        questSuccessPanel[i].SetActive(false);
                    }
                    else
                    {
                        clearImg[i].SetActive(false);
                    }
                }
                //이어하기가 아니면 전부 다 꺼줌
                else
                {
                    clearImg[i].SetActive(false);
                }

            }

            hiddenMissionPanel.SetActive(false);
            isFirst = false;
        }

    }
    IEnumerator DownPanel(int _idx)
    {
        while (true)
        {
            questSuccessPanel[_idx].transform.position = Vector3.Lerp(questSuccessPanel[_idx].transform.position, target, Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
            if (Vector3.Distance(questSuccessPanel[_idx].transform.position, target) <= 5.0f)
            {
                questSuccessPanel[_idx].transform.position = target;
                break;
            }

        }
        target = new Vector3(Camera.main.pixelWidth / 2.0f, Camera.main.pixelHeight + 550f);
        StopCoroutine(DownPanel(_idx));
        StartCoroutine(UpPanel(_idx));
        yield break;
    }
    IEnumerator UpPanel(int _idx)
    {
        while (true)
        {
            questSuccessPanel[_idx].transform.position = Vector3.Lerp(questSuccessPanel[_idx].transform.position, target, Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
            if (Vector3.Distance(questSuccessPanel[_idx].transform.position, target) <= 5.0f)
            {
                questSuccessPanel[_idx].transform.position = target;
                break;
            }

        }
        target = new Vector3(Camera.main.pixelWidth / 2.0f, Camera.main.pixelHeight / 1.2f);
        questSuccessPanel[_idx].SetActive(false);
        StopCoroutine(UpPanel(_idx));
        yield break;
    }

    public void TurnOnClearImg(int _idx)
    {
        clearImg[_idx].SetActive(true);
    }
    //패널열기
    void OpenPanel()
    {
        hiddenMissionPanel.SetActive(true);
    }
    //패널닫기
    void ExitPanel()
    {
        hiddenMissionPanel.SetActive(false);
    }
    //퀘스트 목록
    public void CheckQuest()
    {
        int cnt = 0;
        //노말타워 종류별로있을때
        if (!h_Quest[0])
        {
            for (int i = 0; i < 5; i++)
            {
                if (GameDB.Instance.towerId_Count[i] == 0)
                    break;
                cnt++;
            }
            if (cnt == 5)
            {
                h_Quest[0] = true;
                GameDB.Instance.Gold += 200;
                TurnOnClearImg(0);
                text[0].text = "편식은금물\n -클리어-";
                StartCoroutine(DownPanel(0));
            }
        }
        //매직 마법사 타워1 레어 마법사 타워 1개
        if (!h_Quest[1])
        {
            //6번 11번
            if (GameDB.Instance.towerId_Count[5] > 0 && GameDB.Instance.towerId_Count[10] > 0)
            {
                h_Quest[1] = true;
                GameDB.Instance.Gold += 400;
                TurnOnClearImg(1);
                text[1].text = "마법사 듀오\n -클리어-";
                StartCoroutine(DownPanel(1));
            }
        }
        //매직타워 종류별로 있을때
        if (!h_Quest[2])
        {
            cnt = 0;
            for (int i = 5; i < 10; i++)
            {
                if (GameDB.Instance.towerId_Count[i] == 0)
                    break;
                cnt++;
            }
            if (cnt == 5)
            {
                h_Quest[2] = true;
                GameDB.Instance.Gold += 300;
                TurnOnClearImg(2);
                text[2].text = "파랑파랑해\n -클리어-";
                StartCoroutine(DownPanel(2));
            }
        }
        //레어타워 7개이상
        if (!h_Quest[3])
        {
            if (GameDB.Instance.t_dictionary[RareList.RARE].Count >= 7)
            {
                h_Quest[3] = true;
                GameDB.Instance.Gold += 400;
                TurnOnClearImg(3);
                text[3].text = "언 럭키세븐\n -클리어-";
                StartCoroutine(DownPanel(3));
            }
        }
        //유니크타워 종류별
        if (!h_Quest[4])
        {
            cnt = 0;
            for (int i = 15; i < 20; i++)
            {
                if (GameDB.Instance.towerId_Count[i] == 0)
                    break;
                cnt++;
            }
            if (cnt == 5)
            {
                h_Quest[4] = true;
                GameDB.Instance.Gold += 800;
                TurnOnClearImg(4);
                text[4].text = "이상한 취미\n -클리어-";
                StartCoroutine(DownPanel(4));
            }
        }
        //레어1번,레어2번,레어3번,매직1번,매직2번 있을때
        if (!h_Quest[5])
        {
            if (GameDB.Instance.towerId_Count[5] > 0 && GameDB.Instance.towerId_Count[6] > 0 &&
                GameDB.Instance.towerId_Count[10] > 0 && GameDB.Instance.towerId_Count[11] > 0 && GameDB.Instance.towerId_Count[12] > 0)
            {
                h_Quest[5] = true;
                GameDB.Instance.Gold += 500;
                TurnOnClearImg(5);
                text[5].text = "왕실레인져\n -클리어-";
                StartCoroutine(DownPanel(5));
            }
        }
        //노말3번,노말5번,매직1번,유니크5번 있을때
        if (!h_Quest[6])
        {
            if (GameDB.Instance.towerId_Count[2] > 0 && GameDB.Instance.towerId_Count[4] > 0 &&
                GameDB.Instance.towerId_Count[5] > 0 && GameDB.Instance.towerId_Count[19] > 0)
            {
                h_Quest[6] = true;
                GameDB.Instance.Gold += 500;
                TurnOnClearImg(6);
                text[6].text = "라떼는말이야\n -클리어-";
                StartCoroutine(DownPanel(6));
            }
        }
        //노말5번,레어5번,유니크3번,에픽2번
        if (!h_Quest[7])
        {
            if (GameDB.Instance.towerId_Count[4] > 0 && GameDB.Instance.towerId_Count[14] > 0 &&
            GameDB.Instance.towerId_Count[17] > 0 && GameDB.Instance.towerId_Count[21] > 0)
            {
                h_Quest[7] = true;
                GameDB.Instance.Gold += 1000;
                TurnOnClearImg(7);
                text[7].text = "나쁜녀석들\n -클리어-";
                StartCoroutine(DownPanel(7));
            }

        }
        //에픽제외 여성캐릭만 있을때
        if (!h_Quest[8])
        {
            if (GameDB.Instance.towerId_Count[7] > 0 && GameDB.Instance.towerId_Count[9] > 0 &&
            GameDB.Instance.towerId_Count[3] > 0 && GameDB.Instance.towerId_Count[10] > 0 &&
            GameDB.Instance.towerId_Count[12] > 0 && GameDB.Instance.towerId_Count[13] > 0)
            {
                h_Quest[8] = true;
                GameDB.Instance.Gold += 600;
                TurnOnClearImg(8);
                text[8].text = "소녀시대\n -클리어-";
                StartCoroutine(DownPanel(8));
            }
        }
        //에픽제외 모든 등급1,2번있을때
        if (!h_Quest[9])
        {
            if (GameDB.Instance.towerId_Count[0] > 0 && GameDB.Instance.towerId_Count[1] > 0 &&
           GameDB.Instance.towerId_Count[5] > 0 && GameDB.Instance.towerId_Count[6] > 0 &&
           GameDB.Instance.towerId_Count[10] > 0 && GameDB.Instance.towerId_Count[11] > 0 &&
            GameDB.Instance.towerId_Count[15] > 0 && GameDB.Instance.towerId_Count[16] > 0)
            {
                h_Quest[9] = true;
                GameDB.Instance.Gold += 700;
                TurnOnClearImg(9);
                text[9].text = "1등과2등\n -클리어-";
                StartCoroutine(DownPanel(9));
            }
        }
    }

    public bool IsContinue
    {
        get { return isContinue; }
        set { isContinue = value; }
    }

    public bool IsFirst
    {
        get { return isFirst; }
        set { isFirst = value; }
    }
}
