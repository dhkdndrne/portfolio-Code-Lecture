using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckPanelOpen : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.GameModel.isPanelOpen = true;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.GameModel.isPanelOpen = false;
    }
}
