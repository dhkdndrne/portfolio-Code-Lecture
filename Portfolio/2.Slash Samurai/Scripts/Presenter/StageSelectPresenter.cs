using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectPresenter : MonoBehaviour
{
    [BoxGroup("팝업창")]
    [SerializeField] private GameObject objSelectLevelPanel;
	
    private void Start()
    {
        // 게임 시작했을때 스테이지 선택창을 꺼주고 맵 초기화 해줌
        GameManager.Instance.GameStartSubject.Subscribe(_ =>
        {
            objSelectLevelPanel.SetActive(false);
            GameManager.Instance.LoadMap().Forget();
        }).AddTo(gameObject);
        
        InitScollRecycle().Forget();
    }

    public void Open()
    {
        objSelectLevelPanel.SetActive(true);
        objSelectLevelPanel.transform.GetChild(0).GetComponent<ScrollRect>().verticalNormalizedPosition = 9999;
    }
    
    private async UniTaskVoid InitScollRecycle()
    {
        objSelectLevelPanel.SetActive(true);

        await UniTask.DelayFrame(2);
        
        var scrollController = objSelectLevelPanel.transform.GetChild(0).GetComponent<StageScrollRecycleController>();
        int mapDataAmount = DataManager.Instance.MapSaveDataDic.Count;
        
        for (int i = 0; i < mapDataAmount; i++)
        {
            scrollController.SetStageTableData(new StageCellData() { stage = i + 1 });
        }
        scrollController.Init();
        
        objSelectLevelPanel.SetActive(false);
    }
}
