using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePresenter : MonoBehaviour
{
	[SerializeField] private GameObject titlePanel;
	[SerializeField] private GameObject versionPanel;
	[SerializeField] private GameObject errorPanel;

	public void Init()
	{
		titlePanel.SetActive(true);
		
#if UNITY_ANDROID && !UNITY_EDITOR
		titlePanel.GetComponent<Button>().enabled = false;
#endif

		versionPanel.SetActive(false);
		errorPanel.SetActive(false);
		
		if (Screen.height > 1980)
			Camera.main.orthographicSize = 18;
	}

	public void ReadyToStart() => titlePanel.GetComponent<Button>().enabled = true;

	public void FailedLogin()
	{
		errorPanel.SetActive(true);
	}

	public void OpenVersionPanel()
	{
		versionPanel.SetActive(true);
	}
}