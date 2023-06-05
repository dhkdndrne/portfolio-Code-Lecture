using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
public class ExitPresenter : MonoBehaviour
{
	[SerializeField] private Button okButton;
	[SerializeField] private Button noButton;
	[SerializeField] private GameObject panel;
	private void Start()
	{
		panel.SetActive(true);
		
		okButton.onClick.AddListener(() => Application.Quit());
		noButton.onClick.AddListener(() => panel.SetActive(false));

		this.UpdateAsObservable().Subscribe(_ =>
		{
			if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown( KeyCode.Backspace))
				panel.SetActive(true);
		}).AddTo(gameObject);
		
		panel.SetActive(false);
	}
}