using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopUpType
{
    MISS,
    DAMAGE,
    CRITICAL,
    SHIELD,
    HEAL,
    POISON,
    INVINCIBLE
}

public class DamagePopUpManager : MonoBehaviour
{
    public List<DamagePopUp> popupPool = new List<DamagePopUp>();
    [SerializeField] GameObject damagePopUpPrefab;
    [SerializeField] int index = 0;
    private void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            popupPool.Add(Instantiate(damagePopUpPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<DamagePopUp>());
            popupPool[i].transform.gameObject.SetActive(false);
        }
    }

    public void ShowDamagePopUp<T>(Transform _Transform, T _Damage, PopUpType _popUpType)
    {
        if (popupPool[index].gameObject.activeSelf)
        {
            index = popupPool.Count;
            for (int i = 0; i < 50; i++)
            {
                GameObject obj = Instantiate(damagePopUpPrefab, Vector3.zero, Quaternion.identity, transform);
                popupPool.Add(obj.GetComponent<DamagePopUp>());
                obj.SetActive(false);
            }
        }
        popupPool[index].gameObject.SetActive(true);
        popupPool[index].transform.position = _Transform.position;
        popupPool[index].Setup(_Damage, _popUpType);

        if (index < popupPool.Count - 1) index++;
        else index = 0;
    }
}
