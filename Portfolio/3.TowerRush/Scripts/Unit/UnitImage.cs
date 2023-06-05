using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UnitImage : MonoBehaviour
{
    [SerializeField] Animator animator;
    UnitAbillity abillity;
    public SpriteRenderer headImg;
    public SpriteRenderer armorImg;
    public SpriteRenderer legImg_1;
    public SpriteRenderer legImg_2;
    
    private void Awake()
    {
        abillity = GetComponent<UnitAbillity>();
        animator = GetComponent<Animator>();     
    }
    private void OnEnable()
    {
        animator.SetFloat("MoveSpeed", abillity.VariableSpeed);
    }

    public void ImageChange(Sprite _Head,Sprite _Armor,Sprite _Shoe)
    {
        headImg.sprite = _Head;
        armorImg.sprite = _Armor;
        legImg_1.sprite = _Shoe;
        legImg_2.sprite = _Shoe;
    }

    public void ChangeLayer(int _Num)
    {
        headImg.sortingOrder = _Num;
        armorImg.sortingOrder = _Num -1;
        legImg_1.sortingOrder = _Num -2;
        legImg_2.sortingOrder = _Num -2;
    }

}
