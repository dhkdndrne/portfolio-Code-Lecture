using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FactoryAnimation : MonoBehaviour
{
    [Header("캐릭터")]
    public GameObject character;
    public Transform[] charPos;

    public GameObject scrollEffect;
    public GameObject scrollEffect2;
    int charIndex;
    void OnEnable()
    {
        character.transform.position = charPos[0].position;
        ResetEffectValue();
        charIndex = -1;
        StartCoroutine(CharMoveAnimation());
        StartCoroutine(KillDoMove());
    }

    IEnumerator CharMoveAnimation()
    {
        if (charIndex < charPos.Length - 1) charIndex++;
        else
        {            
            charIndex = 0;
            character.transform.position = charPos[charIndex].position;
            ResetEffectValue();
        }

        character.transform.DOMoveX(charPos[charIndex].position.x, 1);
        yield return character.transform.DOMoveX(charPos[charIndex].position.x, 1).WaitForCompletion();
        yield return new WaitForSeconds(1f);

        //스킬 이펙트
        if (charIndex.Equals(2))
        {
            if(ScrollEffect()) yield return new WaitForSeconds(4f);
            StartCoroutine(CharMoveAnimation());
        }
        //장비
        else if (charIndex.Equals(3))
        {
            StartCoroutine(EquipEffect());
        }
        else
        {
            StartCoroutine(CharMoveAnimation());
        }
    }
    //스크롤 색깔
    bool ScrollEffect()
    {
        if (!LobbyManager.Instance.lobbyUI.isInvenOpen && LobbyManager.Instance.factoryManager.factories[LobbyManager.Instance.factoryManager.currFactoryIndex].equipedItems[1] != null)
        {
            float Hue = 0f;
            Scroll sc = LobbyManager.Instance.factoryManager.factories[LobbyManager.Instance.factoryManager.currFactoryIndex].equipedItems[1] as Scroll;

            if (sc.scrollType.Equals(ScrollType.SPECIAL))
            {
                scrollEffect2.SetActive(true);
            }
            else
            {
                scrollEffect.SetActive(true);
                switch (sc.scrollType)
                {
                    case ScrollType.EVADE:
                        Hue = 0.03658536f;
                        break;
                    case ScrollType.DEFENSE:
                        Hue = 0.09756097f;
                        break;
                    case ScrollType.HEAL:
                        Hue = 0.3048781f;
                        break;
                    case ScrollType.SPEED:
                        Hue = 0.5731707f;
                        break;
                    case ScrollType.SHIELD:
                        Hue = 0.7042683f;
                        break;
                }
                scrollEffect.GetComponent<ChangeParticleColor>().Change(Hue);
                return true;
            }
        }
        return false;
    }

    void ResetEffectValue()
    {
        LobbyManager.Instance.lobbyUI.animBodyImage.materialForRendering.SetFloat("_FadeAmount", 1f);
        LobbyManager.Instance.lobbyUI.animShoeImage.materialForRendering.SetFloat("_FadeAmount", 1f);
        scrollEffect.SetActive(false);
        scrollEffect2.SetActive(false);
    }

    IEnumerator EquipEffect()
    {
        bool second = false;
        float t = 1f;
        while (t > -0.1f)
        {
            t -= 0.8f * Time.deltaTime;
            if (!second) LobbyManager.Instance.lobbyUI.animBodyImage.materialForRendering.SetFloat("_FadeAmount", t);
            else LobbyManager.Instance.lobbyUI.animShoeImage.materialForRendering.SetFloat("_FadeAmount", t);

            if (t <= -0.1f && !second)
            {
                t = 1f;
                second = true;
            }

            yield return null;
        }
        StartCoroutine(CharMoveAnimation());
    }
    IEnumerator KillDoMove()
    {
        yield return new WaitUntil(() => gameObject.activeSelf == false);
        character.transform.DOKill();
    }
}
