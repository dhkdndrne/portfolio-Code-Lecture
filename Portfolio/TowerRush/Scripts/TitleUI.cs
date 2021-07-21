using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    public Text text;
    public Text versionText;
    public GameObject panel;
    public Button confirmButton;
    [SerializeField] RectTransform textBG;

    void Start()
    {
        confirmButton.onClick.AddListener(() => Application.Quit());
        text = GameObject.Find("Text").GetComponent<Text>();
        versionText.text = "version " + Application.version;
    }

    private void Update()
    {
        textBG.sizeDelta = new Vector2(text.rectTransform.rect.width + 100, text.rectTransform.rect.height);
    }

    private void OnDestroy()
    {
        text = null;
        versionText = null;
        panel = null;
        confirmButton = null;
        Resources.UnloadUnusedAssets();
    }
}
