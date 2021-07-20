using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MagicController : MonoBehaviour
{
    public GameObject magicRangeImage;

    [SerializeField] List<MagicSlot> magicSlot = new List<MagicSlot>();
    public List<MagicSlot> MagicSlots
    {
        get { return magicSlot; }
    }

    MagicSlot selectedMagic;    //선택된 마법슬롯
    bool isMagicSelected;
    public bool IsMagicSelected
    {
        get { return isMagicSelected; }
    }

    Vector3 direction;


    private void Start()
    {
        SetMagicSlot();
    }
    private void Update()
    {
        if (selectedMagic != null)
        {
            TouchEvent();
        }
    }
    //마법 슬롯에 마법들을 넣어줌
    public void SetMagicSlot()
    {
        GameObject obj = GameObject.Find("SkillList");
        for (int i = 0; i < DataController.PlayerMagic.Count; i++)
        {
            obj.transform.GetChild(i).gameObject.SetActive(true);
            magicSlot.Add(obj.transform.GetChild(i).GetComponent<MagicSlot>());

            int temp = i;
            magicSlot[i].Init(DataController.PlayerMagic[i]);
            magicSlot[i].GetComponent<Button>().onClick.AddListener(() => SelectMagic(temp));
        }
    }

    void SelectMagic(int _Idx)
    {
        //같은 슬롯을 눌렀을때 꺼준다
        if (magicSlot[_Idx].isSelected == true)
        {
            magicSlot[_Idx].SetState(false);
            isMagicSelected = false;
            selectedMagic = null;
            return;
        }
        magicSlot[_Idx].SetState(true);
        selectedMagic = magicSlot[_Idx];
        isMagicSelected = true;

        for (int i = 0; i < magicSlot.Count; i++)
        {
            if (magicSlot[i].gameObject.activeSelf != false && i != _Idx && magicSlot[i].isSelected == true)
            {
                magicSlot[i].SetState(false);
            }
        }
    }
    void TouchEvent()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (!EventSystem.current.currentSelectedGameObject && !EventSystem.current.IsPointerOverGameObject(touch.fingerId) && selectedMagic.isSelected && selectedMagic.IsReady && selectedMagic.Magic.possessionCount > 0)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    magicRangeImage.SetActive(true);
                    magicRangeImage.transform.localScale = new Vector3(selectedMagic.Magic.magicStat[selectedMagic.Magic.level].radius / 4.17f, selectedMagic.Magic.magicStat[selectedMagic.Magic.level].radius / 4.17f, 1f);
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    magicRangeImage.transform.position = new Vector3(direction.x, direction.y, 0f);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    magicRangeImage.SetActive(false);
                    selectedMagic.UseMagic(direction);
                
                    isMagicSelected = false;
                    selectedMagic = null;
                }
            }
        }
#endif
#if UNITY_EDITOR

        if (!EventSystem.current.IsPointerOverGameObject() && selectedMagic.isSelected && selectedMagic.IsReady && selectedMagic.Magic.possessionCount > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                magicRangeImage.SetActive(true);
                magicRangeImage.transform.localScale = new Vector3(selectedMagic.Magic.magicStat[selectedMagic.Magic.level].radius / 4.17f, selectedMagic.Magic.magicStat[selectedMagic.Magic.level].radius / 4.17f, 1f);
            }
            else if (Input.GetMouseButton(0))
            {
                direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                magicRangeImage.transform.position = new Vector3(direction.x, direction.y, 0f);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                magicRangeImage.SetActive(false);
                selectedMagic.UseMagic(direction);

                isMagicSelected = false;
                selectedMagic = null;
            }
        }
#endif
    }
}
