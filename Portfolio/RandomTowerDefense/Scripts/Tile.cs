using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //타일 아이디
    int id;
    //유닛 생성 되었는지
    public bool isCreated;
    //타일 위에 올라간 유닛
    public GameObject unit;
    //타일 선택 되었는지
    public bool isSelect;
    Color color;

    private void Start()
    {
        //원래색깔 저장
        color = GetComponent<MeshRenderer>().material.color;
        InvokeRepeating("SetColor", 0f, 0.1f);
    }

    void SetColor()
    {
        if (isSelect)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = color;
        }
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }
}
