using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    public int id;
    bool isQuestEnemy;
    public float hp;
    public float maxHp;
    public Slider hpBar;
    public Slider backHpBar;
    public GameObject enemyBody;
    public bool backHpHit;

    private void Start()
    {
        enemyBody = GetComponent<Transform>().gameObject;
    }
    public void GetDamage(float _damage)
    {
        hp -= _damage;
        Invoke("BackHpFun", 0.5f);

        if (hp <= 0)
        {
            if (isQuestEnemy)
            {
                if (id == 97)
                {
                    GameDB.Instance.Qmon1Alive = false;
                    GameDB.Instance.Gold += 100;
                }
                else if (id == 98)
                {
                    GameDB.Instance.Qmon2Alive = false;
                    GameDB.Instance.Gold += 200;
                }
                else
                {
                    GameDB.Instance.Qmon3Alive = false;
                    GameDB.Instance.Gold += 500;
                }
            }
            GameDB.Instance.KillCount++;
            Destroy(enemyBody);
        }
    }
    void Update()
    {

        hpBar.value = Mathf.Lerp(hpBar.value, hp / maxHp, 5f * Time.deltaTime);
        if (backHpHit)
        {
            backHpBar.value = Mathf.Lerp(backHpBar.value, hp / maxHp, 10f * Time.deltaTime);
            if (hpBar.value >= backHpBar.value - 0.01f)
            {
                backHpHit = false;
                backHpBar.value = hpBar.value;
            }
        }
    }

    void BackHpFun()
    {
        backHpHit = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Splash")
        {
            GetDamage(other.GetComponent<Splash>().damage);
        }
        if (other.tag == "Exit")
        {
            if (isQuestEnemy)
            {
                if (id == 97)
                {
                    GameDB.Instance.Qmon1Alive = false;
                }
                else if (id == 98)
                {
                    GameDB.Instance.Qmon2Alive = false;
                }
                else
                {
                    GameDB.Instance.Qmon3Alive = false;
                }
            }

            GameDB.Instance.Hp--;
            //게임오버 
            if(GameDB.Instance.Hp<=0)
            {
                SceneManager.LoadScene("EndingScene");
            }
            Destroy(enemyBody);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Splash")
        {
            if (other.GetComponent<Splash>().longTime == true)
            {
                GetDamage(other.GetComponent<Splash>().damage);
            }

        }
    }
    public bool IsQuestEnemy
    {
        set{ isQuestEnemy = value; }
    }
}
