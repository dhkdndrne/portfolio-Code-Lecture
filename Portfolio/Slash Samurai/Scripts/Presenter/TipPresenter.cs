using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class TipPresenter : MonoBehaviour
{
	[BoxGroup("비디오")]
	[SerializeField] private VideoClip[] videoClips;

	[BoxGroup("비디오")]
	[SerializeField] private VideoPlayer videoPlayer;

	[SerializeField] private Toggle toggle;
	[SerializeField] private GameObject panel;

	private int index;

	public void PlayTip(int stage)
	{
		index = stage switch
		{
			3 => 0,
			5 => 1,
			12 => 2,
			22 => 3,
			_ => -1
		};

		bool checkShowViedo = PlayerPrefs.GetInt($"Toggle_{index}") == 1 ? false : true;
		if (!checkShowViedo || index == -1) return;

		panel.SetActive(true);

		toggle.isOn = true;
		videoPlayer.clip = videoClips[index];
		videoPlayer.Play();

	}

	public void ClosePanel()
	{
		PlayerPrefs.SetInt($"Toggle_{index}", toggle.isOn ? 1 : 0);
		panel.SetActive(false);
	}
}