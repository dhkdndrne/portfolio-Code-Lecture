using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    TextMeshPro textMesh;
    float disappearTimer;
    Color textColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();

    }

    IEnumerator MoveText()
    {
        float moveYSpeed = 10f;
        float moveXSpeed = 5f;

        while (textColor.a > 0)
        {
            transform.position += new Vector3(moveXSpeed, moveYSpeed) * Time.deltaTime;

            disappearTimer -= Time.deltaTime;

            if (disappearTimer < 0)
            {
                float disappearSpeed = 3f;
                textColor.a -= disappearSpeed * Time.deltaTime;
                textMesh.color = textColor;
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void Setup<T>(T _Damage , PopUpType _PopUpType)
    {
        textMesh.SetText(_Damage.ToString());
       
        if (_PopUpType.Equals(PopUpType.DAMAGE)) textMesh.color = new Color(255 / 255f, 163 / 255f, 0);  //데미지
        else if (_PopUpType.Equals(PopUpType.CRITICAL)) textMesh.color = new Color(227 / 255f, 12 / 255f, 24 / 255f); //크리티컬
        else if (_PopUpType.Equals(PopUpType.SHIELD)) textMesh.color = new Color(57 / 255f, 198 / 255f, 229 / 255f); //실드
        else if(_PopUpType.Equals(PopUpType.POISON)) textMesh.color = new Color(74 / 255f, 1 / 255f, 125 / 255f); // 독 데미지
        else if(_PopUpType.Equals(PopUpType.HEAL)) textMesh.color = new Color(29 / 255f, 219 / 255f, 22 / 255f); // 체력 회복
        else if(_PopUpType.Equals(PopUpType.INVINCIBLE)) textMesh.color = Color.red; // 무적
        else textMesh.color = Color.white; // miss

        textColor = textMesh.color;
        disappearTimer = .5f;

        StartCoroutine(MoveText());
    }
}
