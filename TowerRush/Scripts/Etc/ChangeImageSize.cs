using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeImageSize : MonoBehaviour
{
    RectTransform img;
    void Start()
    {
        img = GetComponent<RectTransform>();
        img.sizeDelta = new Vector2(Screen.width, Screen.height);
    }
}
