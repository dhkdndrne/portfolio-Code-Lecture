using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerCtrl : MonoBehaviour
{
    public static GameObject playerCtrl;
    public GameObject towerList;
    //건설할때
    bool isBuildMode;
    int tempId = -9999;
    //똑같은 타워 수
    public int towerCnt = 0;
    //클릭한 타일
    Tile tile;
    UIMgr uiMgr;
    RaycastHit[] hits;
    public GameObject attackRange;
    private void Start()
    {
        playerCtrl = this.gameObject;
        uiMgr = GameObject.Find("UIMgr").GetComponent<UIMgr>();
        attackRange = GameObject.Find("AttackRange").GetComponent<Transform>().gameObject;
        towerList = GetComponent<Transform>().GetChild(0).GetComponent<Transform>().gameObject;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectTile();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            BulidRandomTower();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            UpgradeTower();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SellTower();
        }
    }
    //타워 판매
    public void SellTower()
    {
        if (tile.unit && tile.unit.GetComponent<towerCtrl>().rareList != RareList.EPIC)
        {
            uiMgr.MovePanel(0);
            GameDB.Instance.towerId_Count[tile.unit.GetComponent<towerCtrl>().id - 1]--;
            switch (tile.unit.GetComponent<towerCtrl>().rareList)
            {
                case RareList.NORMAL:
                    SearchSameTower(RareList.NORMAL, tile.unit.GetComponent<towerCtrl>(), false);
                    for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.NORMAL].Count; i++)
                    {
                        if (GameDB.Instance.t_dictionary[RareList.NORMAL][i] == tile.unit.GetComponent<towerCtrl>())
                        {
                            GameDB.Instance.Gold += tile.unit.GetComponent<towerCtrl>().price;
                            Destroy(GameDB.Instance.t_dictionary[RareList.NORMAL][i].onTile.unit);
                            GameDB.Instance.t_dictionary[RareList.NORMAL][i].onTile.isCreated = false;
                            GameDB.Instance.t_dictionary[RareList.NORMAL][i].onTile.isSelect = false;
                            GameDB.Instance.t_dictionary[RareList.NORMAL].Remove(GameDB.Instance.t_dictionary[RareList.NORMAL][i]);
                            break;
                        }
                    }
                    break;
                case RareList.MAGIC:
                    SearchSameTower(RareList.MAGIC, tile.unit.GetComponent<towerCtrl>(), false);
                    for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.MAGIC].Count; i++)
                    {
                        if (GameDB.Instance.t_dictionary[RareList.MAGIC][i] == tile.unit.GetComponent<towerCtrl>())
                        {
                            GameDB.Instance.Gold += tile.unit.GetComponent<towerCtrl>().price;
                            Destroy(GameDB.Instance.t_dictionary[RareList.MAGIC][i].onTile.unit);
                            GameDB.Instance.t_dictionary[RareList.MAGIC][i].onTile.isCreated = false;
                            GameDB.Instance.t_dictionary[RareList.MAGIC][i].onTile.isSelect = false;
                            GameDB.Instance.t_dictionary[RareList.MAGIC].Remove(GameDB.Instance.t_dictionary[RareList.MAGIC][i]);
                            break;
                        }
                    }
                    break;
                case RareList.RARE:
                    SearchSameTower(RareList.RARE, tile.unit.GetComponent<towerCtrl>(), false);
                    for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.RARE].Count; i++)
                    {
                        if (GameDB.Instance.t_dictionary[RareList.RARE][i] == tile.unit.GetComponent<towerCtrl>())
                        {
                            GameDB.Instance.Gold += tile.unit.GetComponent<towerCtrl>().price;
                            Destroy(GameDB.Instance.t_dictionary[RareList.RARE][i].onTile.unit);
                            GameDB.Instance.t_dictionary[RareList.RARE][i].onTile.isCreated = false;
                            GameDB.Instance.t_dictionary[RareList.RARE][i].onTile.isSelect = false;
                            GameDB.Instance.t_dictionary[RareList.RARE].Remove(GameDB.Instance.t_dictionary[RareList.RARE][i]);
                            break;
                        }
                    }
                    break;
                case RareList.UNIQUE:
                    SearchSameTower(RareList.UNIQUE, tile.unit.GetComponent<towerCtrl>(), false);
                    for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.UNIQUE].Count; i++)
                    {
                        if (GameDB.Instance.t_dictionary[RareList.UNIQUE][i] == tile.unit.GetComponent<towerCtrl>())
                        {
                            GameDB.Instance.Gold += tile.unit.GetComponent<towerCtrl>().price;
                            Destroy(GameDB.Instance.t_dictionary[RareList.UNIQUE][i].onTile.unit);
                            GameDB.Instance.t_dictionary[RareList.UNIQUE][i].onTile.isCreated = false;
                            GameDB.Instance.t_dictionary[RareList.UNIQUE][i].onTile.isSelect = false;
                            GameDB.Instance.t_dictionary[RareList.UNIQUE].Remove(GameDB.Instance.t_dictionary[RareList.UNIQUE][i]);
                            break;
                        }
                    }
                    break;
            }
            attackRange.GetComponent<Transform>().position = new Vector3(-999, -999, -999);
        }


    }

    //랜덤타워 건설
    public void BulidRandomTower()
    {
        int randomNum = 0;
        if (tile != null && GameDB.Instance.Gold >= 100 && tile.isSelect && !tile.isCreated)
        {
            randomNum = Random.Range(1, 6);

            tile.unit = Instantiate(GameDB.Instance.normalTower[randomNum - 1], new Vector3(tile.GetComponent<Transform>().position.x,
                tile.GetComponent<Transform>().position.y + 1.2f,
                tile.GetComponent<Transform>().position.z), Quaternion.identity, this.towerList.transform);
            //판매 가격 설정
            tile.unit.GetComponent<towerCtrl>().price = 50;
            tile.unit.GetComponent<towerCtrl>().onTile = tile;
            tile.isCreated = true;
            GameDB.Instance.towerId_Count[tile.unit.GetComponent<towerCtrl>().id - 1]++;
            tile.unit.GetComponent<towerCtrl>().Damage = GameDB.Instance.towerAttackCost[randomNum - 1, GameDB.Instance.normalTowerLV - 1];
            GameDB.Instance.Gold -= 100;
            //노말타워 리스트에 넣어줌
            //GameDB.Instance.normalTowerList.Add(tile.unit.GetComponent<towerCtrl>());
            GameDB.Instance.t_dictionary[tile.unit.GetComponent<towerCtrl>().rareList].Add(tile.unit.GetComponent<towerCtrl>());
            MissionMgr.instance.CheckQuest();
        }
    }

    //타워 업그레이드
    public void UpgradeTower()
    {
        int randomNum = 0;

        if (tile.unit && tile.isSelect && tile.isCreated)
        {
            randomNum = Random.Range(1, 6);

            if (towerCnt >= 2)
            {
                //같은타워 판별해야됨
                switch (tile.unit.GetComponent<towerCtrl>().rareList)
                {
                    case RareList.NORMAL:

                        //타워 리스트에서 지워줌
                        for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.NORMAL].Count; i++)
                        {
                            //리스트중 같은 종류 타워 한개만 지워준다
                            if (GameDB.Instance.t_dictionary[RareList.NORMAL][i] != tile.unit.GetComponent<towerCtrl>() && (GameDB.Instance.t_dictionary[RareList.NORMAL][i].id == tile.unit.GetComponent<towerCtrl>().id))
                            {

                                Destroy(GameDB.Instance.t_dictionary[RareList.NORMAL][i].onTile.unit);
                                GameDB.Instance.t_dictionary[RareList.NORMAL][i].onTile.isCreated = false;
                                GameDB.Instance.t_dictionary[RareList.NORMAL].Remove(GameDB.Instance.t_dictionary[RareList.NORMAL][i]);
                                break;
                            }
                        }
                        //해당 타일 아이디2개 삭제해줌
                        GameDB.Instance.towerId_Count[tile.unit.GetComponent<towerCtrl>().id - 1] -= 2;
                        SearchSameTower(RareList.NORMAL, tile.unit.GetComponent<towerCtrl>(), false);
                        GameDB.Instance.t_dictionary[RareList.NORMAL].Remove(tile.unit.GetComponent<towerCtrl>());
                        //객체 파괴
                        Destroy(tile.unit);

                        tile.unit = Instantiate(GameDB.Instance.magicTower[randomNum - 1], new Vector3(tile.GetComponent<Transform>().position.x,
                       tile.GetComponent<Transform>().position.y + 1.5f,
                       tile.GetComponent<Transform>().position.z), Quaternion.identity, this.towerList.transform);

                        //다음 등급 리스트에 넣어줌
                        GameDB.Instance.t_dictionary[RareList.MAGIC].Add(tile.unit.GetComponent<towerCtrl>());
                        //공격력 재설정
                        tile.unit.GetComponent<towerCtrl>().Damage = GameDB.Instance.towerAttackCost[(randomNum + 4), GameDB.Instance.magicTowerLV - 1];
                        //ui 갱신
                        uiMgr.SetTowerInfoTxt(tile.unit.GetComponent<towerCtrl>().towerName,
                            tile.unit.GetComponent<towerCtrl>().rareList,
                            tile.unit.GetComponent<towerCtrl>().Damage,
                            tile.unit.GetComponent<towerCtrl>().attackSpeed,
                            tile.unit.GetComponent<towerCtrl>().attackType,
                            tile.unit.GetComponent<towerCtrl>().attackCount);
                        //가격 올려줌
                        tile.unit.GetComponent<towerCtrl>().price = 100;
                        tile.unit.GetComponent<towerCtrl>().onTile = tile;
                        GameDB.Instance.towerId_Count[tile.unit.GetComponent<towerCtrl>().id - 1]++;
                        break;

                    case RareList.MAGIC:
                        //타일 리스트에서 지워줌
                        for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.MAGIC].Count; i++)
                        {
                            //리스트중 같은 종류 타일 한개만 지워준다
                            if (GameDB.Instance.t_dictionary[RareList.MAGIC][i] != tile.unit.GetComponent<towerCtrl>() && (GameDB.Instance.t_dictionary[RareList.MAGIC][i].id == tile.unit.GetComponent<towerCtrl>().id))
                            {
                                Destroy(GameDB.Instance.t_dictionary[RareList.MAGIC][i].onTile.unit);
                                GameDB.Instance.t_dictionary[RareList.MAGIC][i].onTile.isCreated = false;
                                GameDB.Instance.t_dictionary[RareList.MAGIC].Remove(GameDB.Instance.t_dictionary[RareList.MAGIC][i]);
                                break;
                            }
                        }
                        GameDB.Instance.towerId_Count[tile.unit.GetComponent<towerCtrl>().id - 1] -= 2;
                        SearchSameTower(RareList.MAGIC, tile.unit.GetComponent<towerCtrl>(), false);
                        GameDB.Instance.t_dictionary[RareList.MAGIC].Remove(tile.unit.GetComponent<towerCtrl>());
                        //객체 파괴
                        Destroy(tile.unit);

                        tile.unit = Instantiate(GameDB.Instance.rareTower[randomNum - 1], new Vector3(tile.GetComponent<Transform>().position.x,
                       tile.GetComponent<Transform>().position.y + 1.5f,
                       tile.GetComponent<Transform>().position.z), Quaternion.identity, this.towerList.transform);

                        //다음 등급 리스트에 넣어줌
                        GameDB.Instance.t_dictionary[RareList.RARE].Add(tile.unit.GetComponent<towerCtrl>());
                        //공격력 재설정
                        tile.unit.GetComponent<towerCtrl>().Damage = GameDB.Instance.towerAttackCost[(randomNum + 9), GameDB.Instance.rareTowerLV - 1];
                        //ui 갱신
                        uiMgr.SetTowerInfoTxt(tile.unit.GetComponent<towerCtrl>().towerName,
                            tile.unit.GetComponent<towerCtrl>().rareList,
                            tile.unit.GetComponent<towerCtrl>().Damage,
                            tile.unit.GetComponent<towerCtrl>().attackSpeed,
                            tile.unit.GetComponent<towerCtrl>().attackType,
                            tile.unit.GetComponent<towerCtrl>().attackCount);
                        //가격 올려줌
                        tile.unit.GetComponent<towerCtrl>().price = 200;
                        tile.unit.GetComponent<towerCtrl>().onTile = tile;
                        GameDB.Instance.towerId_Count[tile.unit.GetComponent<towerCtrl>().id - 1]++;
                        break;

                    case RareList.RARE:
                        //타일 리스트에서 지워줌
                        for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.RARE].Count; i++)
                        {
                            //리스트중 같은 종류 타일 한개만 지워준다
                            if (GameDB.Instance.t_dictionary[RareList.RARE][i] != tile.unit.GetComponent<towerCtrl>() && (GameDB.Instance.t_dictionary[RareList.RARE][i].id == tile.unit.GetComponent<towerCtrl>().id))
                            {
                                Destroy(GameDB.Instance.t_dictionary[RareList.RARE][i].onTile.unit);
                                GameDB.Instance.t_dictionary[RareList.RARE][i].onTile.isCreated = false;
                                GameDB.Instance.t_dictionary[RareList.RARE].Remove(GameDB.Instance.t_dictionary[RareList.RARE][i]);
                                break;
                            }
                        }
                        GameDB.Instance.towerId_Count[tile.unit.GetComponent<towerCtrl>().id - 1] -= 2;
                        SearchSameTower(RareList.RARE, tile.unit.GetComponent<towerCtrl>(), false);
                        GameDB.Instance.t_dictionary[RareList.RARE].Remove(tile.unit.GetComponent<towerCtrl>());
                        //객체 파괴
                        Destroy(tile.unit);

                        tile.unit = Instantiate(GameDB.Instance.uniqueTower[randomNum - 1], new Vector3(tile.GetComponent<Transform>().position.x,
                       tile.GetComponent<Transform>().position.y + 1.5f,
                       tile.GetComponent<Transform>().position.z), Quaternion.identity, this.towerList.transform);

                        //다음 등급 리스트에 넣어줌
                        GameDB.Instance.t_dictionary[RareList.UNIQUE].Add(tile.unit.GetComponent<towerCtrl>());
                        //공격력 재설정
                        tile.unit.GetComponent<towerCtrl>().Damage = GameDB.Instance.towerAttackCost[(randomNum + 14), GameDB.Instance.uniqueTowerLV - 1];
                        //ui 갱신
                        uiMgr.SetTowerInfoTxt(tile.unit.GetComponent<towerCtrl>().towerName,
                            tile.unit.GetComponent<towerCtrl>().rareList,
                            tile.unit.GetComponent<towerCtrl>().Damage,
                            tile.unit.GetComponent<towerCtrl>().attackSpeed,
                            tile.unit.GetComponent<towerCtrl>().attackType,
                            tile.unit.GetComponent<towerCtrl>().attackCount);
                        //가격 올려줌
                        tile.unit.GetComponent<towerCtrl>().price = 400;
                        tile.unit.GetComponent<towerCtrl>().onTile = tile;
                        GameDB.Instance.towerId_Count[tile.unit.GetComponent<towerCtrl>().id - 1]++;
                        break;

                    case RareList.UNIQUE:
                        //타일 리스트에서 지워줌
                        for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.UNIQUE].Count; i++)
                        {
                            //리스트중 같은 종류 타일 한개만 지워준다
                            if (GameDB.Instance.t_dictionary[RareList.UNIQUE][i] != tile.unit.GetComponent<towerCtrl>() && (GameDB.Instance.t_dictionary[RareList.UNIQUE][i].id == tile.unit.GetComponent<towerCtrl>().id))
                            {
                                Destroy(GameDB.Instance.t_dictionary[RareList.UNIQUE][i].onTile.unit);
                                GameDB.Instance.t_dictionary[RareList.UNIQUE][i].onTile.isCreated = false;
                                GameDB.Instance.t_dictionary[RareList.UNIQUE].Remove(GameDB.Instance.t_dictionary[RareList.UNIQUE][i]);
                                break;
                            }
                        }
                        GameDB.Instance.towerId_Count[tile.unit.GetComponent<towerCtrl>().id - 1] -= 2;
                        SearchSameTower(RareList.UNIQUE, tile.unit.GetComponent<towerCtrl>(), false);
                        GameDB.Instance.t_dictionary[RareList.UNIQUE].Remove(tile.unit.GetComponent<towerCtrl>());
                        //객체 파괴
                        Destroy(tile.unit);

                        tile.unit = Instantiate(GameDB.Instance.epicTower[randomNum - 1], new Vector3(tile.GetComponent<Transform>().position.x,
                       tile.GetComponent<Transform>().position.y + 1.5f,
                       tile.GetComponent<Transform>().position.z), Quaternion.identity, this.towerList.transform);

                        //다음 등급 리스트에 넣어줌
                        GameDB.Instance.t_dictionary[RareList.EPIC].Add(tile.unit.GetComponent<towerCtrl>());
                        //공격력 재설정
                        tile.unit.GetComponent<towerCtrl>().Damage = GameDB.Instance.towerAttackCost[(randomNum + 19), GameDB.Instance.epicTowerLV - 1];
                        //ui 갱신
                        uiMgr.SetTowerInfoTxt(tile.unit.GetComponent<towerCtrl>().towerName,
                            tile.unit.GetComponent<towerCtrl>().rareList,
                            tile.unit.GetComponent<towerCtrl>().Damage,
                            tile.unit.GetComponent<towerCtrl>().attackSpeed,
                            tile.unit.GetComponent<towerCtrl>().attackType,
                            tile.unit.GetComponent<towerCtrl>().attackCount);
                        //가격 올려줌
                        tile.unit.GetComponent<towerCtrl>().price = 800;
                        tile.unit.GetComponent<towerCtrl>().onTile = tile;
                        GameDB.Instance.towerId_Count[tile.unit.GetComponent<towerCtrl>().id - 1]++;
                        break;
                }
                attackRange.GetComponent<Transform>().localScale = new Vector3(tile.unit.GetComponent<towerCtrl>().attackRange * 2, tile.unit.GetComponent<towerCtrl>().attackRange * 2, tile.unit.GetComponent<towerCtrl>().attackRange * 2);
                MissionMgr.instance.CheckQuest();
            }
        }
    }

    //똑같은 타워 찾아서 불켜주거나 꺼줌
    void SearchSameTower(RareList _rareList, towerCtrl _tower, bool _onOff)
    {
        towerCnt = 0;
        switch (_rareList)
        {
            case RareList.NORMAL:
                for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.NORMAL].Count; i++)
                {
                    if (GameDB.Instance.t_dictionary[RareList.NORMAL][i].id == _tower.id)
                    {
                        towerCnt++;
                        GameDB.Instance.t_dictionary[RareList.NORMAL][i].OnOffParticle(_onOff);
                    }
                }
                break;

            case RareList.MAGIC:
                for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.MAGIC].Count; i++)
                {
                    if (GameDB.Instance.t_dictionary[RareList.MAGIC][i].id == _tower.id)
                    {
                        towerCnt++;
                        GameDB.Instance.t_dictionary[RareList.MAGIC][i].OnOffParticle(_onOff);
                    }
                }
                break;


            case RareList.RARE:
                for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.RARE].Count; i++)
                {
                    if (GameDB.Instance.t_dictionary[RareList.RARE][i].id == _tower.id)
                    {
                        towerCnt++;
                        GameDB.Instance.t_dictionary[RareList.RARE][i].OnOffParticle(_onOff);
                    }
                }
                break;


            case RareList.UNIQUE:
                for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.UNIQUE].Count; i++)
                {
                    if (GameDB.Instance.t_dictionary[RareList.UNIQUE][i].id == _tower.id)
                    {
                        towerCnt++;
                        GameDB.Instance.t_dictionary[RareList.UNIQUE][i].OnOffParticle(_onOff);
                    }
                }
                break;


            case RareList.EPIC:
                for (int i = 0; i < GameDB.Instance.t_dictionary[RareList.EPIC].Count; i++)
                {
                    if (GameDB.Instance.t_dictionary[RareList.EPIC][i].id == _tower.id)
                    {
                        GameDB.Instance.t_dictionary[RareList.EPIC][i].OnOffParticle(_onOff);
                    }
                }
                break;
        }
    }
    public void ShowTowerInfo()
    {
        if (tile)
        {
            uiMgr.SetTowerInfoTxt(tile.unit.GetComponent<towerCtrl>().towerName,
       tile.unit.GetComponent<towerCtrl>().rareList,
       tile.unit.GetComponent<towerCtrl>().Damage,
       tile.unit.GetComponent<towerCtrl>().attackSpeed,
       tile.unit.GetComponent<towerCtrl>().attackType,
       tile.unit.GetComponent<towerCtrl>().attackCount);
        }

    }
    //타워건설 할 타일 선택
    void SelectTile()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                if (hit.collider.tag == "Tile")
                {
                    //아이디 다를때
                    if (tempId != hit.collider.GetComponent<Tile>().ID)
                    {
                        //처음선택했을때
                        if (tile == null)
                        {
                            tile = hit.collider.GetComponent<Tile>();
                        }
                        //전 타일이 선택되있으면 선택 해제해줌
                        else if (tile.isSelect)
                        {
                            //전 타일위에 타워가있으면 불꺼줌
                            if (tile.GetComponent<Tile>().isCreated)
                            {
                                SearchSameTower(tile.unit.GetComponent<towerCtrl>().rareList, tile.unit.GetComponent<towerCtrl>(), false);
                            }
                            tile.isSelect = false;
                        }

                        //클릭한 타일을 선택되게 값 넣어줌
                        tile = hit.collider.GetComponent<Tile>();
                        tempId = tile.ID;
                        tile.isSelect = true;

                        //타일위에 타워가 있으면
                        if (tile.isCreated)
                        {
                            uiMgr.MovePanel(1);
                            uiMgr.SetTowerInfoTxt(tile.unit.GetComponent<towerCtrl>().towerName,
                                tile.unit.GetComponent<towerCtrl>().rareList,
                                tile.unit.GetComponent<towerCtrl>().Damage,
                                tile.unit.GetComponent<towerCtrl>().attackSpeed,
                                tile.unit.GetComponent<towerCtrl>().attackType,
                                tile.unit.GetComponent<towerCtrl>().attackCount);
                            SearchSameTower(tile.unit.GetComponent<towerCtrl>().rareList, tile.unit.GetComponent<towerCtrl>(), true);
                            attackRange.GetComponent<Transform>().localScale = new Vector3(tile.unit.GetComponent<towerCtrl>().attackRange * 2, tile.unit.GetComponent<towerCtrl>().attackRange * 2, tile.unit.GetComponent<towerCtrl>().attackRange * 2);
                            attackRange.GetComponent<Transform>().position = new Vector3(tile.unit.GetComponent<Transform>().position.x, tile.unit.GetComponent<Transform>().position.y, tile.unit.GetComponent<Transform>().position.z);
                        }
                        //타일위에 타워가 없으면
                        else
                        {
                            attackRange.GetComponent<Transform>().position = new Vector3(-999, -999, -999);
                            uiMgr.MovePanel(0);
                        }
                    }
                    //아이디 같을때
                    else
                    {
                        hit.collider.GetComponent<Tile>().isSelect = true;
                        //타일위에 타워가 있으면
                        if (tile.isCreated)
                        {
                            uiMgr.MovePanel(1);
                            uiMgr.SetTowerInfoTxt(tile.unit.GetComponent<towerCtrl>().towerName,
                                tile.unit.GetComponent<towerCtrl>().rareList,
                                tile.unit.GetComponent<towerCtrl>().Damage,
                                tile.unit.GetComponent<towerCtrl>().attackSpeed,
                                tile.unit.GetComponent<towerCtrl>().attackType,
                                tile.unit.GetComponent<towerCtrl>().attackCount);

                            SearchSameTower(tile.unit.GetComponent<towerCtrl>().rareList, tile.unit.GetComponent<towerCtrl>(), true);
                        }
                        //타일위에 타워가 없으면
                        else
                        {
                            attackRange.GetComponent<Transform>().position = new Vector3(-999, -999, -999);
                            uiMgr.MovePanel(0);
                        }
                    }

                }
                //타일밖 선택했을때
                else
                {
                    //예외처리 안해주면 캐릭터 콜라이더랑 겹쳐서 타일선택이 안됨
                    if (hit.collider.tag == "Wall" || hit.collider.tag == "Floor")
                    {

                        uiMgr.MovePanel(0);
                        attackRange.GetComponent<Transform>().position = new Vector3(-999, -999, -999);
                        if (tile != null && tile.isSelect)
                        {
                            //타일위에 타워가 있으면
                            if (tile.isCreated)
                            {
                                SearchSameTower(tile.unit.GetComponent<towerCtrl>().rareList, tile.unit.GetComponent<towerCtrl>(), false);
                            }
                            tile.isSelect = false;
                        }
                        tempId = -9999;
                    }


                }

            }
        }

    }

}
