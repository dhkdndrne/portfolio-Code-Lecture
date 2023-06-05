using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoUI : MonoBehaviour
{
    [SerializeField] Image towerImage;
    [SerializeField] Text name;
    [SerializeField] Text level;
    [SerializeField] Text damage;
    [SerializeField] Text attackSpeed;
    [SerializeField] Text range;
    [SerializeField] Text debuff;
    [SerializeField] Text description;
    [SerializeField] Text target;
    public void Init(TowerBase _Tower)
    {
        towerImage.sprite = _Tower.gameObject.GetComponent<SpriteRenderer>().sprite;

        name.text = _Tower.towerName;
        level.text = "레벨: " + _Tower.level.ToString();
        damage.text = "공격력: " + _Tower.damage.ToString();

        if (_Tower.towerName.Equals("RandomTower")) attackSpeed.text = "공격속도: 0 ~ 3 무작위";
        else attackSpeed.text = "공격속도: " + _Tower.fireDelay.ToString();

        range.text = "공격범위: " + _Tower.range.ToString();
        debuff.text = "디버프 효과: " + _Tower.debuffDescription;
        description.text = "설명: " + _Tower.description;

        switch (_Tower.targetOption)
        {
            case TargetOption.NEAR:
                target.text = "타겟: 가까운 적";
                break;

            case TargetOption.RANDOM:
                target.text = "타겟: 무작위";
                break;

            case TargetOption.HIGH_HP:
                target.text = "타겟: 체력 높은 적";
                break;

            case TargetOption.LOW_HP:
                target.text = "타겟: 체력 낮은 적";
                break;
        }

    }
}
