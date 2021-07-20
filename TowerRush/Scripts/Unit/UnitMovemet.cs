using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitMovemet : MonoBehaviour
{

    UnitAbillity abillity;
    Vector3 target;
    List<Transform> waypoints = new List<Transform>();
    int targetIndex;

    Vector3 prevTarget; // 좌우반전용
    GameObject unitBody;

    public GameObject stunEffect;   //스턴 이펙트
    public Button clearBindButton;  //속박해제 버튼
    private void Awake()
    {
        abillity = GetComponent<UnitAbillity>();
        unitBody = transform.GetChild(0).gameObject;
        clearBindButton.onClick.AddListener(ClearBind);
        Transform[] temp = GameObject.Find("WayPointC").GetComponentsInChildren<Transform>();
        for (int i = 1; i < temp.Length; i++)
        {
            waypoints.Add(temp[i]);
        }
    }

    private void OnEnable()
    {
        stunEffect.SetActive(false);
        clearBindButton.gameObject.SetActive(false);
        StartCoroutine(CheckBind());

        targetIndex = 0;
        transform.position = waypoints[targetIndex++].position;
        target = waypoints[targetIndex].position;

        //좌우반전  //왼쪽에서 시작하면 왼쪽을 바라보게 아니면 오른쪽
        if (target.x < transform.position.x)
        {
            unitBody.transform.localScale = new Vector3(-1, 1, 1);
            clearBindButton.transform.localScale = new Vector3(-1, 1, 1);
            
        }
        else
        {
            unitBody.transform.localScale = new Vector3(1, 1, 1);
            clearBindButton.transform.localScale = new Vector3(1, 1, 1);
        }
        StartCoroutine(MoveChar());
    }

    IEnumerator MoveChar()
    {
        while (Vector3.Distance(transform.position, target) > 1f)
        {
            Vector3 dir = target - transform.position;
            transform.Translate(dir.normalized * abillity.VariableSpeed * Time.deltaTime);
            yield return null;
        }
        GetNextWaypoint();
    }

    void GetNextWaypoint()
    {
        if (targetIndex >= waypoints.Count - 1)
        {
            EndPath();
            return;
        }
        targetIndex++;
        prevTarget = target;
        target = waypoints[targetIndex].position;

        if (target.x < transform.position.x)
        {
            unitBody.transform.localScale = new Vector3(-1, 1, 1);
            clearBindButton.transform.localScale = new Vector3(-1, 1, 1);
            abillity.hpBar.HPImage.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            unitBody.transform.localScale = new Vector3(1, 1, 1);
            clearBindButton.transform.localScale = new Vector3(1, 1, 1);
            abillity.hpBar.HPImage.transform.localScale = new Vector3(1, 1, 1);
        }
        StartCoroutine(MoveChar());
    }

    void EndPath()
    {
        GameManager.Instance.targetCount--;
        abillity.Die();
        if (GameManager.Instance.targetCount.Equals(0))
        {
            GameManager.Instance.EndStage(true);
        }
    }

    IEnumerator CheckBind()
    {
        yield return new WaitUntil(() => abillity.isBind);
        clearBindButton.gameObject.SetActive(true);
        stunEffect.SetActive(true);
    }

    public void ClearBind()
    {
        if(!UserData.Instance.userdata.isFinishTutorial)
        {
            GameManager.Instance.tutorialManager.check = true;
        }

        abillity.isBind = false;
        stunEffect.SetActive(false);
        clearBindButton.gameObject.SetActive(false);
        StartCoroutine(abillity.ClearBuffOnBind());
        StartCoroutine(CheckBind());
    }
}
